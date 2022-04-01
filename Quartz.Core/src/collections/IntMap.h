#ifndef QUARTZ_CORE_INTMAP_H
#define QUARTZ_CORE_INTMAP_H

#include "SortedIntIntList.h"

#define isEven(x) (x & 1) == 0

class IntMap {
public:
    SortedIntIntList even;
    SortedIntIntList odd;
    
    [[nodiscard]] 
    size_t count() const {return even.elements.size() + odd.elements.size();}

    void set(IntInt element);
    void remove(uint32_t key);
    
    [[nodiscard]]
    uint32_t tryGetValue(uint32_t key) const;

    void set(uint32_t key, uint32_t val);
    void clear();
};


#endif //QUARTZ_CORE_INTMAP_H
