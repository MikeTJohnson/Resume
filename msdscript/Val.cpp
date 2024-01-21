#include "expr.h"
#include "val.h"
#include "pointers.h"
#include "env.h"

/**
 *\file Val.cpp
 *\brief An object that represents the value of a mathematical expression
 *
 *An object that represents an interpreted expression. That value takes the form of a number, true, or false.
 *\author Michael Johnson
 */

/**
 *\brief NumVal constructor
 *\param val Initial number value the object is set to
 */
NumVal::NumVal(int val) {
    this -> val = val;
}

/**
 *\brief Evaluates whether this value is equivelant to another value
 *\param *v A value to check equivelancy against
 *\return True if the 2 values are equal, false if the 2 values are not equal
 */
bool NumVal::equals(PTR(Val) v) {
    PTR(NumVal) n = CAST(NumVal)(v);
    if (n == NULL) {
        return false;
    }
    else {
        return (val == n -> val);
    }
}

/**
 *\brief Helper method to add 2 values together
 *\param *v The value object to add to the orginial value
 *\return The new value object equal to the original values added together
 */
PTR(Val) NumVal::addTo(PTR(Val) v) {
    PTR(NumVal) n = CAST(NumVal)(v);
    if (n == NULL) {
        throw std::runtime_error("non-value addition");
    }
    else {
        return NEW(NumVal)((unsigned)val + (unsigned)n->val);
    }
}

/**
 *\brief Helper method to multiply 2 values together
 *\param *v The value object to multiply to the original value
 *\return The new value object equal to the original values multiplied together
 */
PTR(Val) NumVal::multTo(PTR(Val) v) {
    PTR(NumVal) n = CAST(NumVal)(v);
    if (n == NULL) {
        throw std::runtime_error("non-value multiplication");
    }
    else {
        return NEW(NumVal)((unsigned)val * (unsigned)n->val);
    }
}

/**
 *\brief Turns a value object into an expression object
 *\return New number expression with the value as an initialization
 */
PTR(Expr) NumVal::toExpr() {
    return NEW(NumExpr)(val);
}

/**
 *\brief Turns a value object into a string
 *\return The value contained in the object as a string
 */
std::string NumVal::toString() {
    return std::to_string(val);
}

/**
 *\brief Determines if a value object is true or false
 *\return Always throws an exeption for a number value
 */
bool NumVal::isTrue() {
    throw std::runtime_error("non-boolean comparison");
}

PTR(Val) NumVal::call(PTR(Val) argument) {
    throw std::runtime_error("cannot call number value");
}

//------------------------------------------------

/**
 *\brief BoolVal constructor
 *\param value True or false value
 */
BoolVal::BoolVal(bool value) {
    val = value;
}

/**
 *\brief Evaluates whether this value is equal to another value
 *\param *v An expression to compare against
 *\return True if the values are equal, false if the values are not equal
 */
bool BoolVal::equals(PTR(Val) v) {
    PTR(BoolVal) b = CAST(BoolVal)(v);
    if (b == NULL) {
        return false;
    }
    else {
        return (val == b -> val);
    }
}

/**
 *\brief Helper method to add 2 values together
 *\param *v The value object to add to the orginial value
 *\return Always throws an exception for a boolean value
 */
PTR(Val) BoolVal::addTo(PTR(Val) v) {
    throw std::runtime_error("cannot add boolean value");
}

/**
 *\brief Helper method to add 2 values together
 *\param *v The value object to multiply to the orginial value
 *\return Always throws an exception for a boolean value
 */
PTR(Val) BoolVal::multTo(PTR(Val) v) {
    throw std::runtime_error("non-value multiplication");
}

/**
 *\brief Turns a value object into an expression object
 *\return New boolean expression with the value as an initialization
 */
PTR(Expr) BoolVal::toExpr() {
    return NEW(BoolExpr)(val);
}

/**
 *\brief Turns a value object into a string
 *\return The value contained in the object as a string
 */
std::string BoolVal::toString() {
    if (val) {
        return "_true";
    }
    else {
        return "_false";
    }
}

/**
 *\brief Determines if a value object is true or false
 *\return True or false as determined by the boolean value
 */
bool BoolVal::isTrue() {
    return val;
}

PTR(Val) BoolVal::call(PTR(Val) argument) {
    throw std::runtime_error("cannot call boolean value");
}

//------------------------------------------------

_funVal::_funVal(std::string argument, PTR(Expr) body, PTR(Env) env) {
    this -> argument = argument;
    this -> body = body;
    this -> env = env;
}

bool _funVal::equals(PTR(Val) v) {
    PTR(_funVal) f = CAST(_funVal)(v);
    if (f == NULL) {
        return false;
    }
    else {
        return (argument == f->argument &&
                body -> equals(f->body));
    }
}

PTR(Val) _funVal::addTo(PTR(Val) v) {
    throw std::runtime_error("cannot add function values");
}

PTR(Val) _funVal::multTo(PTR(Val) v) {
    throw std::runtime_error("cannot multiply function values");
}

PTR(Expr) _funVal::toExpr() {
    return NEW(_funExpr)(argument, body);
}

std::string _funVal::toString() {
    std::string function = "(fun(" + argument + ") (" + body -> interp(NEW(EmptyEnv)()) -> toString() + "))";
    return function;
}

bool _funVal::isTrue() {
    throw std::runtime_error("non-boolean comparison of function");
}

PTR(Val) _funVal::call(PTR(Val) argument) {
    return body -> interp(NEW(ExtendedEnv)(this->argument, argument, env));
}

Val::~Val(){}
