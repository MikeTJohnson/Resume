//
//  Expr.hpp
//  msdscript
//
//  Created by Michael Johnson on 1/19/23.
//

#pragma once
#include <stdio.h>
#include <string>
#include <stdexcept>
#include <sstream>
#include "pointers.h"
/**
 *\file Expr.hpp
 *\brief An object that represents a mathmatical expression
 *
 *An object that represents a number, variable, or the application of addition or multiplication. The object does not need to be represented strictly by numbers as long as variables are included. Variables should be substituted for number expressions before any interpretation is performed
 *\author Michael Johnson
 */

/**
 *\brief Simple enumeration denoting the precedence of expressions
 */
typedef enum {
    noPrec,
    addPrec,
    multPrec,
    letPrec
} ExprPrec;

/**
 *\brief Simple declaration to use value objects in class methods
 */
class Val;
class Env;

/**
 \brief Parent class to all other expressions
 */
CLASS(Expr){
public:
    virtual bool equals(PTR(Expr) e) = 0;
    virtual PTR(Val) interp(PTR(Env) env) = 0;
    virtual bool hasVariable() = 0;
    virtual void print(std::ostream&) = 0;
    virtual void prettyPrint(std::ostream&, ExprPrec, std::streampos, bool) = 0;
    void prettyPrintAs(std::ostream& out);
    std::string toPrettyString();
    std::string toString();
    PTR(Expr) parseNumExpr(std::istream&);
    PTR(Expr) parseExpr(std::istream &in);
    std::string parseStr(std::string s);
    virtual ~Expr();
};

/**
 *\brief An expression that represents a whole number
 */
class NumExpr : public Expr{
public:
    int val;/// The number value for the object
    
    NumExpr(int val);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    bool hasVariable();
    void print(std::ostream&);
    void prettyPrint(std::ostream&, ExprPrec, std::streampos, bool);
};

/**
 *\brief An expression that represents 2 other expressions being added together
 */
class AddExpr : public Expr{
public:
    PTR(Expr) lhs;/// The expression on the left of the addition sign "+"
    PTR(Expr) rhs;/// The expression on the right of the addition sign "+"
    
    AddExpr(PTR(Expr) lhs, PTR(Expr) rhs);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    bool hasVariable();
    void print(std::ostream&);
    void prettyPrint(std::ostream&, ExprPrec, std::streampos, bool);
};

/**
 *\brief An expression that represents 2 other expressions being multiplied together
 */
class MultExpr : public Expr{
public:
    PTR(Expr) lhs;/// The expression on the left of the multiplication sign "*"
    PTR(Expr) rhs;/// The expression on the right of the multiplication sign "*"
    
    MultExpr(PTR(Expr) lhs, PTR(Expr) rhs);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    bool hasVariable();
    void print(std::ostream&);
    void prettyPrint(std::ostream&, ExprPrec, std::streampos, bool);
};

/**
 *\brief An expression that represents a variable
 */
class VarExpr : public Expr{
public:
    std::string str;/// The symbol or label the variable is represented by
    
    VarExpr(std::string str);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    bool hasVariable();
    void print(std::ostream&);
    void prettyPrint(std::ostream&, ExprPrec, std::streampos, bool);
};

/**
 *\brief An expression that represents a value being substituted into another expressions stated variable
 */
class _letExpr : public Expr{
public:
    std::string str;/// The variable to substitue
    PTR(Expr) lhs;/// The expression to replace the variable with
    PTR(Expr) rhs;/// The expression with  the variable to be replaced
    
    _letExpr(std::string str, PTR(Expr) lhs, PTR(Expr) rhs);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    bool hasVariable();
    void print(std::ostream&);
    void prettyPrint(std::ostream&, ExprPrec, std::streampos, bool);
};

/**
 *\brief An expression that represents that represents a boolean condition and the steps to take after the condition is interpreted
 */
class _ifExpr : public Expr{
public:
    PTR(Expr) var;/// The boolean expression
    PTR(Expr) t;/// The option to take if the boolean interprets to true
    PTR(Expr) e;/// The option to take if the boolean interprets to false
    
    _ifExpr(PTR(Expr) v, PTR(Expr) t, PTR(Expr) e);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    bool hasVariable();
    void print(std::ostream&);
    void prettyPrint(std::ostream&, ExprPrec, std::streampos, bool);
};

/**
 *\brief an expression that represents true or false
 */
class BoolExpr : public Expr{
public:
    bool value;/// The contained value of the expression. Must be true or false
    
    BoolExpr(bool tof);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    bool hasVariable();
    void print(std::ostream&);
    void prettyPrint(std::ostream&, ExprPrec, std::streampos, bool);
};

/**
 *\brief An expression that represents the equivalency of 2 seperate expressions seperated by a '=='
 */
class EqExpr : public Expr{
public:
    PTR(Expr) rhs;/// The expression on the right side of the '=='
    PTR(Expr) lhs;/// The expression on the left side of the '=='
    
    EqExpr(PTR(Expr) lhs, PTR(Expr) rhs);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    bool hasVariable();
    void print(std::ostream&);
    void prettyPrint(std::ostream&, ExprPrec, std::streampos, bool);
};

class _funExpr : public Expr {
public:

    std::string argument;
    PTR(Expr) body;
    
    _funExpr(std::string argument, PTR(Expr) body);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    bool hasVariable();
    void print(std::ostream&);
    void prettyPrint(std::ostream&, ExprPrec, std::streampos, bool);
};

class callExpr : public Expr {
public:
    
    PTR(Expr) toBeCalled;
    PTR(Expr) argument;
    
    callExpr(PTR(Expr) toBeCalled, PTR(Expr) argument);
    bool equals(PTR(Expr) e);
    PTR(Val) interp(PTR(Env) env);
    bool hasVariable();
    void print(std::ostream&);
    void prettyPrint(std::ostream&, ExprPrec, std::streampos, bool);
};
