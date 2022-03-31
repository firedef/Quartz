#include "library.h"

#include <iostream>

int main() {
    std::cout << "Hello, World!" << std::endl;

    uint8_t* aPtr = MemoryAllocator::allocateGlobal(90);
    std::cout << (uint64_t) aPtr << std::endl;

    uint8_t* bPtr = MemoryAllocator::allocateGlobal(80);
    std::cout << (uint64_t) bPtr << std::endl;
    
    MemoryAllocator::freeGlobal(aPtr);
    
    aPtr = MemoryAllocator::allocateGlobal(100);
    std::cout << (uint64_t) aPtr << std::endl;

    aPtr = MemoryAllocator::allocateGlobal(80);
    std::cout << (uint64_t) aPtr << std::endl;
    
    MemoryAllocator::cleanupGlobal();
}
