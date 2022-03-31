#include "MemoryAllocator.h"

MemoryAllocator* MemoryAllocator::instance;

Allocation::Allocation(uint8_t* allocationPtr, uint32_t allocatedBytes) : allocationPtr(allocationPtr),
                                                                       allocatedBytes(allocatedBytes) {}

MemoryBlock::MemoryBlock(uint32_t start, uint32_t length) : start(start), length(length) {}

int Bucket::findBestBlockId(uint32_t size) {
    uint32_t bestCount = UINT32_MAX;
    int bestIndex = -1;

    for (int i = 0; i < blockCount; ++i) {
        uint32_t s = freeBlocks[i].length;
        if (s >= bestCount || s < size) continue;

        bestCount = s;
        bestIndex = i;
    }

    return bestIndex;
}

void Bucket::recalculate() {
    minFreeBlockSize = UINT32_MAX;
    maxFreeBlockSize = 0;

    for (int i = 0; i < blockCount; ++i) {
        uint32_t s = freeBlocks[i].length;
        if (s < minFreeBlockSize) minFreeBlockSize = s;
        if (s > maxFreeBlockSize) maxFreeBlockSize = s;
    }
}

void Bucket::trimBlock(int id, uint32_t size) {
    MemoryBlock* block = &freeBlocks[id];
    [[unlikely]]
    if (size == block->length) {
        block->length = 0;
        return;
    }

    block->start += size;
    block->length -= size;
}

Allocation Bucket::allocItem(uint32_t size) {
    [[unlikely]]
    if (blockCount == 0) {
        uint32_t spaceLeft = bucketAllocation.allocatedBytes - size;
        freeBlocks.emplace_back(size, spaceLeft);
        blockCount++;
        minFreeBlockSize = maxFreeBlockSize = spaceLeft;
        allocations[bucketAllocation.allocationPtr] = size;
        return {bucketAllocation.allocationPtr, size};
    }

    int bestBlock = findBestBlockId(size);
    uint8_t* ptr = bucketAllocation.allocationPtr + freeBlocks[bestBlock].start;

    trimBlock(bestBlock, size);
    recalculate();

    allocations[ptr] = size;

    return {ptr, size};
}

void Bucket::freeItem(uint8_t *ptr) {
    uint32_t start = ptr - bucketAllocation.allocationPtr;
    uint32_t size = allocations[ptr];
    allocations.erase(ptr);
    freeBlocks.emplace_back(start, size);
    blockCount++;

    if (size < minFreeBlockSize) minFreeBlockSize = size;
    if (size > maxFreeBlockSize) maxFreeBlockSize = size;
}

bool Bucket::compareBlocks(MemoryBlock a, MemoryBlock b) { return b.length == 0 || a.start < b.start; }

void Bucket::cleanup() {
    std::sort(freeBlocks.begin(), freeBlocks.end(), compareBlocks);
    for (int i = blockCount - 1; i >= 0; --i) {
        if (freeBlocks[i].length != 0) break;
        freeBlocks.pop_back();
        blockCount--;
    }

    for (int i = 0; i < blockCount - 1; ++i) {
        if (freeBlocks[i + 1].start != freeBlocks[i].end()) continue;
        freeBlocks[i].length += freeBlocks[i + 1].length;
        freeBlocks.erase(freeBlocks.begin() + i + 1);
        blockCount--;
        i--;
    }

    recalculate();
}

Bucket::Bucket(const Allocation &bucketAllocation) : bucketAllocation(bucketAllocation) {
    minFreeBlockSize = maxFreeBlockSize = bucketAllocation.allocatedBytes;
    blockCount = 0;
    freeBlocks = std::vector<MemoryBlock>();
    allocations = std::unordered_map<uint8_t*, uint32_t>();
}


void MemoryAllocator::GenBucket(uint32_t size) {
    auto* bucketAllocationPtr = (uint8_t*) malloc(size);
    Allocation bucketAllocation(bucketAllocationPtr, size);
    buckets.push_back(new Bucket(bucketAllocation));
    bucketCount++;
}

void MemoryAllocator::DeleteBucket(Bucket* bucket) {
    delete bucket;
}

MemoryAllocator::~MemoryAllocator() {
    for (int i = 0; i < bucketCount; ++i) DeleteBucket(buckets[i]);
}

int MemoryAllocator::findPreferredBucket(uint32_t size) {
    uint32_t preferredCount = UINT32_MAX;
    int preferredIndex = -1;
    
    for (int i = 0; i < bucketCount; ++i) {
        uint32_t minBlock = buckets[i]->minFreeBlockSize;
        if (minBlock >= preferredCount || minBlock < size) continue;
        
        preferredCount = minBlock;
        preferredIndex = i;
    }

    return preferredIndex;
}

uint8_t *MemoryAllocator::allocItem(uint32_t size) {
    int bucket = findPreferredBucket(size);
    [[likely]] if (bucket != -1) return buckets[bucket]->allocItem(size).allocationPtr;

    GenBucket(defaultBucketSize);
    return buckets[bucketCount - 1]->allocItem(size).allocationPtr;
}

int MemoryAllocator::findBucket(uint8_t* ptr) {
    for (int i = 0; i < bucketCount; ++i)
        if (buckets[i]->allocations.contains(ptr)) return i;
    return -1;
}

void MemoryAllocator::freeItem(uint8_t *ptr) {
    int bucket = findBucket(ptr);
    
#ifdef LOG_ALLOC_ERRORS
    if (bucket == -1) std::cout << "cannot free allocation: bucket not found\n";
#endif
    
    if (bucket != -1) buckets[bucket]->freeItem(ptr);
}

MemoryAllocator::MemoryAllocator(uint32_t defaultBucketSize) : defaultBucketSize(defaultBucketSize) {
    
}

MemoryAllocator::MemoryAllocator() : defaultBucketSize(DEFAULT_ALLOCATOR_BUCKET_SIZE) {}

void MemoryAllocator::cleanupGlobal() {getInstance()->cleanup();}

void MemoryAllocator::freeGlobal(uint8_t *ptr) {getInstance()->free(ptr);}

uint8_t* MemoryAllocator::allocateGlobal(uint32_t sizeBytes) { return getInstance()->allocate(sizeBytes);}

MemoryAllocator* MemoryAllocator::getInstance() {
    [[unlikely]] if (instance == nullptr) instance = new MemoryAllocator();
    return instance;
}

void MemoryAllocator::cleanup() {for (int i = 0; i < bucketCount; ++i) buckets[i]->cleanup();}

void MemoryAllocator::free(uint8_t *ptr) {return freeItem(ptr);}

uint8_t* MemoryAllocator::allocate(uint32_t sizeBytes) {return allocItem(sizeBytes);}
