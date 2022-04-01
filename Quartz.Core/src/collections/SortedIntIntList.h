#ifndef QUARTZ_CORE_SORTEDINTINTLIST_H
#define QUARTZ_CORE_SORTEDINTINTLIST_H

#include <cinttypes>
#include <vector>

struct IntInt {
    uint32_t key;
    uint32_t val;
};

class SortedIntIntList {
public:
    std::vector<IntInt> elements;

    [[nodiscard]] 
    int binarySearch(uint32_t key) const;
    
    [[nodiscard]] 
    int binarySearchExact(uint32_t key) const;

    void insert(int index, IntInt element);
    void removeAt(int index);
    
    void set(IntInt element);
    void remove(uint32_t key);
    
    [[nodiscard]] 
    uint32_t tryGetValue(uint32_t key) const;
};


#endif //QUARTZ_CORE_SORTEDINTINTLIST_H
