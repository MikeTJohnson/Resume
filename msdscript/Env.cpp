#include "Env.hpp"
#include "pointers.h"

EmptyEnv::EmptyEnv() {
}

PTR(Val) EmptyEnv::lookup(std::string findName) {
    throw std::runtime_error("free variable: " + findName);
}

//----------------------------------

ExtendedEnv::ExtendedEnv(std::string name, PTR(Val) val, PTR(Env) rest) {
    this -> name = name;
    this -> val = val;
    this -> rest = rest;
}

PTR(Val) ExtendedEnv::lookup(std::string findName) {
    if (findName == name) {
        return val;
    }
    else {
        return rest->lookup(findName);
    }
}
