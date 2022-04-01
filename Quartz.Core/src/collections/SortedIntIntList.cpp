#include "SortedIntIntList.h"

int SortedIntIntList::binarySearch(const uint32_t key) const {
    int size = (int) elements.size();
    [[unlikely]]
    if (size == 0) return -1;
    
    int low = 0, mid;
    int high = size - 1;

    while (low <= high) {
        mid = low + ((high - low) >> 1);

        [[unlikely]]
        if (elements[mid].key == key) return mid;
        if (elements[mid].key < key) {
            low = mid + 1;
            continue;
        } 
        high = mid - 1;
    }

    if (low < 0 || low >= elements.size()) return -1;
    
    return low;
}

int SortedIntIntList::binarySearchExact(const uint32_t key) const {
    int index = binarySearch(key);
    if (index != -1 && elements[index].key == key) return index;
    return -1;
}

void SortedIntIntList::insert(int index, IntInt element) {
    elements.insert(elements.begin() + index, element);
}

void SortedIntIntList::remove(uint32_t key) {
    int index = binarySearchExact(key);
    [[likely]] if (index > -1) removeAt(index);
}

void SortedIntIntList::set(IntInt element) {
    int index = binarySearch(element.key);
    
    [[unlikely]]
    if (index == -1) {
        elements.push_back(element);
        return;
    }

    if (elements[index].key == element.key) {
        elements[index].val = element.val;
        return;
    } 

    insert(index, element);
}

void SortedIntIntList::removeAt(const int index) {
    elements.erase(elements.begin() + index);
}

uint32_t SortedIntIntList::tryGetValue(const uint32_t key) const {
    int index = binarySearchExact(key);
    [[likely]] if (index > -1) return elements[index].val;
    return UINT32_MAX;
}
