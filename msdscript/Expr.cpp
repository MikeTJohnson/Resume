//
//  Expr.cpp
//  msdscript
//
//  Created by Michael Johnson on 1/19/23.
//

#include "Val.hpp"
#include "Expr.hpp"
#include "catch.h"
#include <sstream>
#include <stdexcept>
#include "pointers.h"
#include "Env.hpp"
/**
 *\file Expr.cpp
 *\brief An object that represents a mathmatical expression
 *
 *An object that represents a number, variable, or the application of addition or multiplication. The object does not need to be represented strictly by numbers as long as variables are included. Variables should be substituted for number expressions before any interpretation is performed
 *\author Michael Johnson
 */

/**
 *\brief NumExpr constructor
 *\param val Initial value the object is set to
 */
NumExpr::NumExpr(int val) {
    this -> val = val;
}

/**
 *\brief Evaluates whether this expression is equal to another expression
 *\param *e An expression to compare values against
 *\return True if the expressions are equal, false if the expressions are not equal
 */
bool NumExpr::equals(PTR(Expr) e) {
    PTR(NumExpr) n = CAST(NumExpr)(e);
    if (n == NULL) {
        return false;
    }
    else {
        return (val == n -> val);
    }
}

/**
 *\brief Interperates the value of the expression in a mathematical manner
 *\return The value of this expression
 */
PTR(Val) NumExpr::interp(PTR(Env) env) {
    return NEW(NumVal)(val);
}

/**
 *\brief Checks if this expression contains a variable
 *\return True if the value contains a variable, false if it does not contain a variable
 */
bool NumExpr::hasVariable() {
    return false;
}

/**
 *\brief Prints the expression in a human readable form
 *\param out The desired stream to ouput the expression
 */
void NumExpr::print(std::ostream& out) {
    out << std::to_string(val);
}

/**
 *\brief Prints the expression in human readable form without unneccessary parentheses
 *\param out The desired stream to output the expression
 *\param e The precedence of the calling function
 *\param streamPos Current position of the stream
 *\param paranth Boolean determining whether the method needs to add parantheses
 */
void NumExpr::prettyPrint(std::ostream& out, ExprPrec e, std::streampos streamPos, bool paranth) {
    out << std::to_string(val);
}

//---------------------------------------------

/**
 *\brief AddExpr constructor
 *\param *lhs The left hand side sub-expression of the add expression
 *\param *rhs The right hand side sub-expression of the add expression
 */
AddExpr::AddExpr(PTR(Expr) lhs, PTR(Expr) rhs) {
    this -> lhs = lhs;
    this -> rhs = rhs;
}

/**
 *\brief Evaluates whether this expression is equal to another expression
 *\param *e An expression to compare values against
 *\return True if the expressions are equal, false if the expressions are not equal
 */
bool AddExpr::equals(PTR(Expr) e) {
    PTR(AddExpr) a = CAST(AddExpr)(e);
    if (a == NULL) {
        return false;
    }
    else {
        return (lhs ->equals( a -> lhs ) && rhs ->equals( a -> rhs));
    }
}

/**
 *\brief Interperates the value of the expression in a mathematical manner
 *\return The value of this expression
 */
PTR(Val) AddExpr::interp(PTR(Env) env) {
    return (lhs -> interp(env) -> addTo(rhs -> interp(env)));
}

/**
 *\brief Checks if this expression contains a variable
 *\return True if the expression contains a variable, false if it does not contain a variable
 */
bool AddExpr::hasVariable() {
    if (lhs -> hasVariable()) {
        return true;
    }
    else if (rhs -> hasVariable()) {
        return true;
    }
    else {
        return false;
    }
}

/**
 *\brief Prints the expression in a human readable form
 *\param out The desired stream to ouput the expression
 */
void AddExpr::print(std::ostream& out) {
    out << "(";
    lhs -> print(out);
    out << "+";
    rhs -> print(out);
    out << ")";
}

/**
 *\brief Prints the expression in human readable form without unneccessary parentheses
 *\param out The desired stream to output the expression
 *\param e The precedence of the calling function
 *\param streamPos Current position of the stream
 *\param paranth Boolean determining whether the method needs to add parantheses
 */
void AddExpr::prettyPrint(std::ostream& out, ExprPrec e, std::streampos streamPos, bool paranth) {
    if (e >= addPrec) {
        out << "(";
    }
    lhs -> prettyPrint(out, addPrec, streamPos, true);
    out << " + ";
    rhs -> prettyPrint(out, noPrec, streamPos, false);

    if (e >= addPrec) {
        out << ")";
    }
}

//---------------------------------------------

/**
 *\brief MultExpr constructor
 *\param *lhs The left hand side sub-expression of the add expression
 *\param *rhs The right hand side sub-expression of the add expression
 */
MultExpr::MultExpr(PTR(Expr) lhs, PTR(Expr) rhs) {
    this -> lhs = lhs;
    this -> rhs = rhs;
}

/**
 *\brief Evaluates whether this expression is equal to another expression
 *\param *e An expression to compare values against
 *\return True if the expressions are equal, false if the expressions are not equal
 */
bool MultExpr::equals(PTR(Expr) e) {
    PTR(MultExpr) m = CAST(MultExpr)(e);
    if (m == NULL) {
        return false;
    }
    else {
        return (lhs ->equals( m -> lhs) && rhs ->equals( m -> rhs));
    }
}

/**
 *\brief Interperates the value of the expression in a mathematical manner
 *\return The value of this expression
 */
PTR(Val) MultExpr::interp(PTR(Env) env){
    return (lhs -> interp(env) -> multTo(rhs -> interp(env)));
}

/**
 *\brief Checks if this expression contains a variable
 *\return True if the expression contains a variable, false if it does not contain a variable
 */
bool MultExpr::hasVariable() {
    if (lhs -> hasVariable()) {
        return true;
    }
    else if (rhs -> hasVariable()) {
        return true;
    }
    else {
        return false;
    }
}

/**
 *\brief Prints the expression in a human readable form
 *\param out The desired stream to ouput the expression
 */
void MultExpr::print(std::ostream& out) {
    out << "(";
    lhs -> print(out);
    out << "*";
    rhs -> print(out);
    out << ")";
}

/**
 *\brief Prints the expression in human readable form without unneccessary parentheses
 *\param out The desired stream to output the expression
 *\param e The precedence of the calling function
 *\param streamPos Current position of the stream
 *\param paranth Boolean determining whether the method needs to add parantheses
 */
void MultExpr::prettyPrint(std::ostream& out, ExprPrec e, std::streampos streamPos, bool paranth) {
    if (e == multPrec) {
        out << "(";
    }
    lhs -> prettyPrint(out, multPrec, streamPos, true);
    out << " * ";

    if (e == addPrec) {
        rhs -> prettyPrint(out, addPrec, streamPos, true);
    }
    else {
        rhs -> prettyPrint(out, addPrec, streamPos, false);
    }
    if (e == multPrec) {
        out << ")";
    }
}

//---------------------------------------------

/**
 *\brief VarExpr constructor
 *\param *str The initial representation of the variable
 */
VarExpr::VarExpr(std::string str) {
    this -> str = str;
}

/**
 *\brief Evaluates whether this expression is equal to another expression
 *\param *e An expression to compare values against
 *\return True if the expressions are equal, false if the expressions are not equal
 */
bool VarExpr::equals(PTR(Expr) e) {
    PTR(VarExpr) v = CAST(VarExpr)(e);
    if (v == NULL) {
        return false;
    }
    else {
        return (str == v -> str);
    }
}

/**
 *\brief Interperates the value of the expression in a mathematical manner
 *\return throws an error if the variable has not been given an expression substitution
 */
PTR(Val) VarExpr::interp(PTR(Env) env) {
    return env->lookup(str);
//    throw std::runtime_error("no value for variable");
}

/**
 *\brief Checks if this expression contains a variable
 *\return True
 */
bool VarExpr::hasVariable() {
    return true;
}

/**
 *\brief Prints the expression in a human readable form
 *\param out The desired stream to ouput the expression
 */
void VarExpr::print(std::ostream& out) {
    out << str;
}

/**
 *\brief Prints the expression in human readable form without unneccessary parentheses
 *\param out The desired stream to output the expression
 *\param e The precedence of the calling function
 *\param streamPos Current position of the stream
 *\param paranth Boolean determining whether the method needs to add parantheses
 */
void VarExpr::prettyPrint(std::ostream& out, ExprPrec e, std::streampos streamPos, bool paranth) {
    out << str;
}


//---------------------------------------------

/**
 *\brief Driver method for prettyPrint testing
 *\return String output to the terminal
 */
void Expr::prettyPrintAs(std::ostream& out) {
    std::streampos streamPosition = 0;
    THIS -> prettyPrint(out, noPrec, streamPosition, false);
}

/**
 *\brief Prints the expression in a human readable form to the terminal
 *\return The stream to the terminal with the expression loaded
 */
std::string Expr::toString() {
    std::stringstream ss;
    THIS->print(ss);
    return ss.str();
}

/**
 *\brief Prints the expression in a human readable form to the terminal without unneccessary parantheses
 *\return the stream to the terminal with the expression loaded
 */
std::string Expr::toPrettyString() {
    std::streampos streamPosition = 0;
    std::stringstream ss("");
    THIS -> prettyPrint(ss, noPrec, streamPosition, false);
    return ss.str();
}

Expr::~Expr() {}


//---------------------------------------------

/**
 *\brief Let expression constructor
 *\param v String depicting the variable to be replaced
 *\param lhs The expression to replace the variable with
 *\param rhs The body expression containing the variable to be replaced
 */
_letExpr::_letExpr(std::string v, PTR(Expr) lhs, PTR(Expr) rhs) {
    this -> str = v;
    this -> lhs = lhs;
    this -> rhs = rhs;
}

/**
 *\brief Evaluates whether this expression is equal to another expression
 *\param e The expression to compare for equivalence
 *\return true if the expressions are equal, false if the expressions are not equal
 */
bool _letExpr::equals(PTR(Expr) e) {
    PTR(_letExpr) l = CAST(_letExpr)(e);
    if (l == NULL) {
        return false;
    }
    else {
        return (str == l -> str &&
                lhs ->equals( l -> lhs) &&
                rhs ->equals( l -> rhs));
    }
}

/**
 *\brief Mathematically evaluates the let expression by substituting the variable with the lhs expression
 *\return Whole number value of the evaluated let expression
 */
PTR(Val) _letExpr::interp(PTR(Env) env) {
//    return rhs->subst(str, lhs)->interp();
    PTR(Val) lhsVal = lhs -> interp(env);
    PTR(Env) envReplace = NEW(ExtendedEnv)(str, lhsVal, env);
    return rhs -> interp(envReplace);
}

/**
 *\brief Evaluates if the expression contains a variable in the body expression
 *\return Returns true if the body contains a variable, false if the body does not contain a variable
 */
bool _letExpr::hasVariable() {
    return rhs->hasVariable();
}

/**
 *\brief Prints the let expression in human readable form to the desired output
 *\param out The output stream to print the expression
 */
void _letExpr::print(std::ostream& out) {
    out << "(_let " << str << "=";
    lhs->print(out);
    out << " _in ";
    rhs->print(out);
    out << ")";
}

/**
 *\brief Prints the expression in human readable form without unneccessary parentheses
 *\param out The desired stream to output the expression
 *\param e The precedence of the calling function
 *\param streamPos Current position of the stream
 *\param paranth Boolean determining whether the method needs to add parantheses
 */
void _letExpr::prettyPrint(std::ostream& out, ExprPrec e, std::streampos streamPos, bool paranth) {
    if (paranth) {
        out << "(";
    }
    long currentPos = (long)out.tellp();
    long indent = currentPos - streamPos;
    out << "_let " << str << " = ";
    lhs->prettyPrint(out, noPrec, streamPos, paranth);
    out << "\n";
    streamPos = out.tellp();
    out << std::string(indent, ' ');
    out << "_in  ";
    rhs->prettyPrint(out, noPrec, streamPos, paranth);
    if (paranth) {
        out << ")";
    }
}


//---------------------------------------------


/**
 *\brief if expression constructor
 *\param *v The boolean expression
 *\param *t The option to take if the boolean interprets to true
 *\param *e The option to take if the boolean interprets to false
 */
_ifExpr::_ifExpr(PTR(Expr) v, PTR(Expr) t, PTR(Expr) e) {
    var = v;
    this -> t = t;
    this -> e = e;
}

/**
 *\brief Evaluates whether this expression is equal to another expression
 *\param *e The expression to compare for equivalence
 *\return true if the expressions are equal, false if the expressions are not equal
 */
bool _ifExpr::equals(PTR(Expr) e) {
    PTR(_ifExpr) i = CAST(_ifExpr)(e);
    if (i == NULL) {
        return false;
    }
    else {
        return (var ->equals( i -> var) &&
                t ->equals( i -> t) &&
                e ->equals( i -> e));
    }
}

/**
 *\brief Mathematically evaluates the if expression and executes 1 of the other expressions depending on the boolean value
 *\return The value of the first expression if true, and the value of the second expression if false
 */
PTR(Val) _ifExpr::interp(PTR(Env) env) {
    PTR(Val) value = var->interp(env);
    if(value->isTrue()) {
        return t->interp(env);
    }
    else {
        return e->interp(env);
    }
}

/**
 *\brief Determines if the expression contains a variable
 *\return True if the expression contains a variable, false if the expression does not contain a variable
 */
bool _ifExpr::hasVariable() {
    return (var->hasVariable()||
            t->hasVariable()||
            e->hasVariable());
}

/**
 *\brief Prints the if expression to the output destination
 *\param out the output destination
 */
void _ifExpr::print(std::ostream& out) {
    out << "(_if ";
    var->print(out);
    out << " _then ";
    t->print(out);
    out << " _else ";
    e->print(out);
    out << ")";
}

/**
 *\brief Prints the if expression without unneccessary parantheses
 *\param out The output destination
 *\param prec The precedence of the expression
 *\param pos The current position of the stream
 *\param tof Boolean determining if this method needs to add parantheses
 *\return Always throws an exception for if expressions
 */
void _ifExpr::prettyPrint(std::ostream& out, ExprPrec prec, std::streampos pos, bool tof) {
    throw std::runtime_error("cannot prettyprint _ifExpr");
}


//---------------------------------------------


/**
 *\brief Boolean expression constructor
 *\param tof The value to initialize the expression with. Must be true or false
 */
BoolExpr::BoolExpr(bool tof) {
    value = tof;
}

/**
 *\brief Evaluates whether this expression is equal to another expression
 *\param *e The expression to compare for equivalence
 *\return true if the expressions are equal, false if the expressions are not equal
 */
bool BoolExpr::equals(PTR(Expr) e){
    PTR(BoolExpr) b = CAST(BoolExpr)(e);
    if (b == NULL) {
        return false;
    }
    else {
        return value == b -> value;
    }
}

/**
 *\brief Determines if the value of the boolean expression is true or false
 *\return The boolean value of the expression
 */
PTR(Val) BoolExpr::interp(PTR(Env) env) {
    return NEW(BoolVal)(value);
}

/**
 *\brief Determines if the expression contains a variable
 *\return Always returns false for boolean expressions
 */
bool BoolExpr::hasVariable() {
    return false;
}

/**
 *\brief Prints the boolean expression to the output destination
 *\param out the output destination
 */
void BoolExpr::print(std::ostream& out) {
    if (value == true) {
        out << "true";
    }
    else {
        out << "false";
    }
}

/**
 *\brief Prints the boolean expression without unneccessary parantheses
 *\param out The output destination
 *\param prec The precedence of the expression
 *\param pos The current position of the stream
 *\param tof Boolean determining if this method needs to add parantheses
 *\return Always throws an exception for boolean expressions
 */
void BoolExpr::prettyPrint(std::ostream& out, ExprPrec prec, std::streampos pos, bool tof) {
    throw std::runtime_error("cannot prettyprint boolean");
}


//---------------------------------------------


/**
 *\brief Equal expression constructor
 *\param *lhs The expression on the left of the '=='
 *\param *rhs The expression on the right of the '=='
 */
EqExpr::EqExpr(PTR(Expr) lhs, PTR(Expr) rhs) {
    this -> rhs = rhs;
    this -> lhs = lhs;
}

/**
 *\brief Evaluates whether this expression is equal to another expression
 *\param *e The expression to compare for equivalence
 *\return true if the expressions are equal, false if the expressions are not equal
 */
bool EqExpr::equals(PTR(Expr) e) {
    PTR(EqExpr) eq = CAST(EqExpr)(e);
    if (eq == NULL) {
        return false;
    }
    else {
        return (rhs -> equals(eq -> rhs) &&
                lhs -> equals(eq -> lhs));
    }
}

/**
 *\brief Interprets if the values of the rhs and lhs are equal
 *\return True if the values are equal, false if the values are not equal
 */
PTR(Val) EqExpr::interp(PTR(Env) env) {
    if (lhs -> interp(env) -> equals(rhs -> interp(env))) {
        return NEW(BoolVal)(true);
    }
    else {
        return NEW(BoolVal)(false);
    }
}

/**
 *\brief Determines if the expression contains a variable
 *\return True if the expression contains a variable, false if the expression does not contain a variable
 */
bool EqExpr::hasVariable() {
    return (lhs -> hasVariable() ||
            rhs -> hasVariable());
}

/**
 *\brief Prints the boolean expression to the output destination
 *\param out the output destination
 */
void EqExpr::print(std::ostream& out) {
    out << "(";
    lhs -> print(out);
    out << " == ";
    rhs -> print(out);
    out << ")";
}

/**
 *\brief Prints the boolean expression without unneccessary parantheses
 *\param out The output destination
 *\param prec The precedence of the expression
 *\param pos The current position of the stream
 *\param tof Boolean determining if this method needs to add parantheses
 *\return Always throws an exception for equal expressions
 */
void EqExpr::prettyPrint(std::ostream& out, ExprPrec prec, std::streampos pos, bool tof) {
    throw std::runtime_error("cannot prettyprint equal expression");
}


//---------------------------------------------


_funExpr::_funExpr(std::string argument, PTR(Expr) body) {
    this -> argument = argument;
    this -> body = body;
}

bool _funExpr::equals(PTR(Expr) e) {
    PTR(_funExpr) fun = CAST(_funExpr)(e);
    if (fun == NULL) {
        return false;
    }
    else {
        return (argument == fun->argument &&
                body -> equals(fun -> body));
    }
}

PTR(Val) _funExpr::interp(PTR(Env) env) {
    return NEW(_funVal)(argument, body, env);
}

bool  _funExpr::hasVariable() {
    return body -> hasVariable();
}

void _funExpr::print(std::ostream& out) {
    out << "(fun(" << argument << ") ";
    body -> print(out);
    out << ")";
}

void _funExpr::prettyPrint(std::ostream& out, ExprPrec prec, std::streampos pos, bool parantheses) {
    throw std::runtime_error("cannot pretty print function expression");
}


//---------------------------------------------


callExpr::callExpr(PTR(Expr) toBeCalled, PTR(Expr) argument) {
    this -> toBeCalled = toBeCalled;
    this -> argument = argument;
}

bool callExpr::equals(PTR(Expr) e) {
    PTR(callExpr) call = CAST(callExpr)(e);
    if (call == NULL) {
        return false;
    }
    else {
        return (toBeCalled -> equals(call ->toBeCalled) &&
                argument -> equals(call -> argument));
    }
}

PTR(Val) callExpr::interp(PTR(Env) env) {
    return toBeCalled -> interp(env) -> call(argument -> interp(env));
}

bool callExpr::hasVariable() {
    return toBeCalled -> hasVariable();
}

void callExpr::print(std::ostream& out) {
    out << "(";
    toBeCalled -> print(out);
    out << ")(";
    argument -> print(out);
    out << ")";
}

void callExpr::prettyPrint(std::ostream& out, ExprPrec prec, std::streampos pos, bool parantheses) {
    throw std::runtime_error("not done");
}
