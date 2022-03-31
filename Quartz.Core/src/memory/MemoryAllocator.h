#ifndef QUARTZ_CORE_MEMORYALLOCATOR_H
#define QUARTZ_CORE_MEMORYALLOCATOR_H

#include <cstddef>
#include <cinttypes>
#include <vector>
#include <unordered_map>
#include <iostream>

#define LOG_ALLOC_ERRORS
#define DEFAULT_ALLOCATOR_BUCKET_SIZE 1<<28

struct Allocation {
    uint8_t* allocationPtr;
    uint32_t allocatedBytes;

    Allocation(uint8_t* allocationPtr, uint32_t allocatedBytes);
};

struct MemoryBlock {
    uint32_t start;
    uint32_t length;
    
    [[nodiscard]] uint32_t end() const { return start + length;}

    MemoryBlock(uint32_t start, uint32_t length);
};

class Bucket {
public:
    Allocation bucketAllocation;
    
    uint32_t minFreeBlockSize;
    uint32_t maxFreeBlockSize;
    int blockCount;
    
    std::vector<MemoryBlock> freeBlocks;
    std::unordered_map<uint8_t*, uint32_t> allocations;

    explicit Bucket(const Allocation &bucketAllocation);
    
    int findBestBlockId(uint32_t size);
    void recalculate();
    void trimBlock(int id, uint32_t size);
    Allocation allocItem(uint32_t size);
    void freeItem(uint8_t* ptr);
    static bool compareBlocks(MemoryBlock a, MemoryBlock b);
    void cleanup();
};

class MemoryAllocator {
private:
    static MemoryAllocator* instance;
    
    uint32_t bucketCount{};
    uint64_t currentAllocated{};
    uint64_t totalAllocated{};
    
    uint32_t defaultBucketSize;
    
    std::vector<Bucket*> buckets; 
    
    void GenBucket(uint32_t size);
    static inline void DeleteBucket(Bucket* bucket);

    int findPreferredBucket(uint32_t size);
    inline uint8_t* allocItem(uint32_t size);
    
    inline int findBucket(uint8_t* ptr);
    inline void freeItem(uint8_t* ptr);
    
public:
    MemoryAllocator();
    explicit MemoryAllocator(uint32_t defaultBucketSize);

    virtual ~MemoryAllocator();
    
    uint8_t* allocate(uint32_t sizeBytes);
    void free(uint8_t* ptr);
    void cleanup();
    
    static MemoryAllocator* getInstance();
    
    static uint8_t* allocateGlobal(uint32_t sizeBytes);
    static void freeGlobal(uint8_t* ptr);
    static void cleanupGlobal();
};


#endif //QUARTZ_CORE_MEMORYALLOCATOR_H
