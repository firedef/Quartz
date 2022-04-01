#include "library.h"

#include "src/collections/IntMap.h"
#include <iostream>

int main() {
    IntMap map{};
    map.set(4, 33);
    map.set(7, 42);
    map.set(1, 78);
    map.set(2, 8678699);
    map.set(12, 54);
    map.set(6, 23);
    
    std::cout << map.even.elements.size() << std::endl;
    std::cout << map.odd.elements.size() << std::endl;

    std::cout << "---" << std::endl;
    
    std::cout << map.tryGetValue(7) << std::endl;
    std::cout << map.tryGetValue(1) << std::endl;
    std::cout << map.tryGetValue(-2) << std::endl;
    std::cout << map.tryGetValue(6) << std::endl;
    std::cout << map.tryGetValue(2) << std::endl;
    
    std::cout << "---" << std::endl;
    
    map.remove(2);
    std::cout << map.tryGetValue(2) << std::endl;
    std::cout << map.tryGetValue(6) << std::endl;
    std::cout << map.tryGetValue(4) << std::endl;
}
