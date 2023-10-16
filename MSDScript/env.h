#pragma once
#include "pointers.h"
#include <string>

class Val;

CLASS(Env) {
public:
    virtual PTR(Val) lookup(std::string findName) = 0;
};

class EmptyEnv : public Env {

public:
    EmptyEnv();
    PTR(Val) lookup(std::string findName);
};

class ExtendedEnv : public Env {
    std::string name;
    PTR(Val) val;
    PTR(Env) rest;

public:
    ExtendedEnv(std::string name, PTR(Val) val, PTR(Env) rest);
    PTR(Val) lookup(std::string findName);
};
