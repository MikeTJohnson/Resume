#pragma once
#include <stdio.h>
#include <string>
#include "pointers.h"

/**
 *\file Val.cpp
 *\brief An object that represents the value of a mathematical expression
 *
 *An object that represents an interpreted expression. That value takes the form of a number, true, or false.
 *\author Michael Johnson
 */

/**
 *\brief Simple declaration to use value objects in class methods
 */
class Expr;
class Env;

/**
 *\brief Parent class to all other values
 */
CLASS(Val) {
public:

    virtual bool equals(PTR(Val) v) = 0;
    virtual PTR(Val) addTo(PTR(Val) v) = 0;
    virtual PTR(Val) multTo(PTR(Val) v) = 0;
    virtual PTR(Expr) toExpr() = 0;
    virtual std::string toString() = 0;
    virtual bool isTrue() = 0;
    virtual PTR(Val) call(PTR(Val) argument) = 0;
    virtual ~Val();
};

/**
 *\brief A value that represents a Number
 */
class NumVal : public Val {
public:
    int val;/// The number value for the object

    NumVal(int value);
    bool equals(PTR(Val) v);
    PTR(Val) addTo(PTR(Val) v);
    PTR(Val) multTo(PTR(Val) v);
    PTR(Expr) toExpr();
    std::string toString();
    bool isTrue();
    PTR(Val) call(PTR(Val) argument);
};

/**
 *\brief A value that represents a boolean
 */
class BoolVal : public Val {
public:
    bool val;/// The boolean value for the object

    BoolVal(bool value);
    bool equals(PTR(Val) v);
    PTR(Val) addTo(PTR(Val) v);
    PTR(Val) multTo(PTR(Val) v);
    PTR(Expr) toExpr();
    std::string toString();
    bool isTrue();
    PTR(Val) call(PTR(Val) argument);
};

class _funVal : public Val {
public:

    std::string argument;
    PTR(Expr) body;
    PTR(Env) env;

    _funVal(std::string argument, PTR(Expr) body, PTR(Env) env);
    bool equals(PTR(Val) v);
    PTR(Val) addTo(PTR(Val) v);
    PTR(Val) multTo(PTR(Val) v);
    PTR(Expr) toExpr();
    std::string toString();
    bool isTrue();
    PTR(Val) call(PTR(Val) argument);
};



