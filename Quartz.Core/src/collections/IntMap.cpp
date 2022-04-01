#include "IntMap.h"
 
void IntMap::set(IntInt element) {
    if (isEven(element.key)) even.set(element);
    else odd.set(element);
}

void IntMap::remove(uint32_t key) {
    if (isEven(key)) even.remove(key);
    else odd.remove(key);
}

uint32_t IntMap::tryGetValue(uint32_t key) const {
    if (isEven(key)) return even.tryGetValue(key);
    return odd.tryGetValue(key);
}

void IntMap::set(uint32_t key, uint32_t val) { set(IntInt{key, val});}

void IntMap::clear() {
    even.elements.clear();
    odd.elements.clear();
}
