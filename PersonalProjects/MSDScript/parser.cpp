#include "parser.h"
#include <iostream>
#include "expr.h"
#include "val.h"
#include "pointers.h"
/**
 *\file parser.cpp
 *\brief A tool to parse expressions written by the user into expressions that are usable by the script
 *
 *Takes user written mathematical expressions and tokenizes them. Once the user input has been tokenized, the tokens are placed into expression objects and sent through the respective interp methods
 *\author Michael Johnson
 */

/**
 *\brief Places number tokens into NumExpr objects
 *\param in The user input to parse
 *\return New NumExpr object
 */
PTR(Expr) parseNum(std::istream &in) {
    int n = 0;
    bool negative = false;
    if (in.peek() == '-') {
        negative = true;
        consume(in, '-');
    }
    while (1) {
        int c = in.peek();
        if (isdigit(c)) {
            consume(in, c);
            n = n*10 + (c - '0');
        }
        else {
            break;
        }
    }
    if (negative) {
        n = -n;
    }
    else if (n > INT_MAX || n < INT_MIN) {
        throw std::runtime_error("user input out of range");
    }
    return NEW(NumExpr)(n);
}

/**
 *\brief Places variable tokens into VarExpr objects
 *\param in The user input to parse
 *\return New VarExpr object
 */
PTR(Expr) parseVar(std::istream &in) {
    std::string variable = "";
    while (1) {
        char next = in.peek();
        if (isalpha(next)) {
            consume(in, next);
            variable += next;
        }
        else {
            break;
        }
    }
    return NEW(VarExpr)(variable);
}

/**
 *\brief Transfers istream to parseNum, parseVar, parseKeyword, or back to parseExpr
 *\param in The user input to parse
 *\return An expression object with a tokenized value
 */
PTR(Expr) parseInner(std::istream &in) {
    skipWhitespace(in);
    int c = in.peek();
    if ((c == '-') || isdigit(c)) {
        return parseNum(in);
    }
    else if (c == '(') {
        consume(in, '(');
        PTR(Expr) e = parseExpr(in);
        skipWhitespace(in);
        c = in.get();
        if (c != ')') {
            throw std::runtime_error("missing close parenthesis");
        }
        return e;
    }
    else if (isalpha(c)) {
        return parseVar(in);
    }
    else if (c == '_') {
        return parseKeyword(in);
    }
    else {
        consume(in, c);
        throw std::runtime_error("invalid input");
    }
}

/**
 *\brief Transfers istream to parseMulticand or parseAddend
 *\param in The user input to parse
 *\return An expression object with a tokenized value
 */
PTR(Expr) parseAddend(std::istream &in) {
    PTR(Expr) lhs = parseMulticand(in);
    skipWhitespace(in);
    int c = in.peek();
    if (c == '*') {
        consume(in, '*');
        skipWhitespace(in);
        PTR(Expr) rhs = parseAddend(in);
        return NEW(MultExpr)(lhs, rhs);
    }
    else {
        return lhs;
    }
}

/**
 *\brief Advances the cursor of the user input as long the next character matches the expectation
 *\param in The user input to parse
 *\param expect The exectation of the next character
 */
void consume(std::istream &in, int expect) {
    int c = in.get();
    if (c != expect){
        throw std::runtime_error("consume mismatch");
    }
}

/**
 *\brief Skips over whitespace to reach the next character
 *\param in The user input to parse
 */
void skipWhitespace(std::istream &in) {
    while (1) {
        int c = in.peek();
        if (!isspace(c)) {
            break;
        }
        consume(in, c);
    }
}

/**
 *\brief Transfers istream to parseAddend or parseComparg
 *\param in The user input to parse
 *\return An expression object with a tokenized value
 */
PTR(Expr) parseComparg(std::istream& in) {
    PTR(Expr) e;
    e = parseAddend(in);
    skipWhitespace(in);
    int c = in.peek();
    if (c == '+') {
        consume(in, '+');
        PTR(Expr) rhs = parseComparg(in);
        return NEW(AddExpr)(e, rhs);
    }
    else {
        return e;
    }
}

/**
 *\brief Transfers istream to parseComparg or parseExpr. Starts the parse process
 *\param in The user input to parse
 *\return An expression object with a tokenized value
 */
PTR(Expr) parseExpr(std::istream &in) {
    PTR(Expr) e;
    e = parseComparg(in);
    skipWhitespace(in);
    int c = in.peek();
    if (c == '=') {
        consume(in, '=');
        consume(in, '=');
        skipWhitespace(in);
        PTR(Expr) rhs = parseExpr(in);
        return NEW(EqExpr)(e, rhs);
    }
    else {
        return e;
    }
}

/**
 *\brief Transfers istream to parseLet, parseTrue, parseFalse, or parseIf
 *\param in The user input to parse
 *\return An expression object with a tokenized value
 */
PTR(Expr) parseKeyword(std::istream& in) {
    consume(in, '_');
    int c = in.peek();
    if (c == 'l') {
        return parseLet(in);
    }
    else if (c == 't') {
        return parseTrue(in);
    }
    else if (c == 'f') {
        consume(in, 'f');
        c = in.peek();
        if (c == 'a') {
            return parseFalse(in);
        }
        else if (c == 'u') {
            return parseFun(in);
        }
        else {
            throw std::runtime_error("invalid keyword");
        }
    }
    else if (c == 'i') {
        return parseIf(in);
    }
    else {
        throw std::runtime_error("invalid keyword");
    }
}

/**
 *\brief Tokenizes the pieces of the let expression and places them in an expression object
 *\param in The user input to parse
 *\return A let expression
 */
PTR(Expr) parseLet(std::istream& in) {
    consume(in, 'l');
    consume(in, 'e');
    consume(in, 't');
    skipWhitespace(in);
    PTR(VarExpr) var = CAST(VarExpr)(parseVar(in));
    skipWhitespace(in);
    consume(in, '=');
    skipWhitespace(in);
    PTR(Expr) lhs = parseExpr(in);
    skipWhitespace(in);
    consume(in, '_');
    consume(in, 'i');
    consume(in, 'n');
    skipWhitespace(in);
    PTR(Expr) rhs = parseExpr(in);
    return NEW(_letExpr)(var->str, lhs, rhs);
}

/**
 *\brief Tokenizes the pieces of a true expression and places them in an expression objection
 *\param in The user input to parse
 *\return A true expression
 */
PTR(Expr) parseTrue(std::istream& in) {
    consume(in, 't');
    consume(in, 'r');
    consume(in, 'u');
    consume(in, 'e');
    return NEW(BoolExpr)(true);
}

/**
 *\brief Tokenizes the pieces of a false expression and places them in an expression objection
 *\param in The user input to parse
 *\return A false expression
 */
PTR(Expr) parseFalse(std::istream& in) {
    consume(in, 'a');
    consume(in, 'l');
    consume(in, 's');
    consume(in, 'e');
    return NEW(BoolExpr)(false);
}

/**
 *\brief Tokenizes the pieces of the if expression and places them in an expression object
 *\param in The user input to parse
 *\return An if expression
 */
PTR(Expr) parseIf(std::istream& in) {
    consume(in, 'i');
    consume(in, 'f');
    skipWhitespace(in);
    PTR(Expr) challenge = parseExpr(in);
    skipWhitespace(in);
    consume(in, '_');
    consume(in, 't');
    consume(in, 'h');
    consume(in, 'e');
    consume(in, 'n');
    skipWhitespace(in);
    PTR(Expr) ifTrue = parseExpr(in);
    skipWhitespace(in);
    consume(in, '_');
    consume(in, 'e');
    consume(in, 'l');
    consume(in, 's');
    consume(in, 'e');
    skipWhitespace(in);
    PTR(Expr) ifFalse = parseExpr(in);
    return NEW(_ifExpr)(challenge, ifTrue, ifFalse);
}

PTR(Expr) parseMulticand(std::istream& in) {
    PTR(Expr) inner = parseInner(in);
    while (in.peek() == '(') {
        consume(in, '(');
        PTR(Expr) argument = parseExpr(in);
        consume(in, ')');
        return NEW(callExpr)(inner, argument);
    }
    return inner;
}

PTR(Expr) parseFun(std::istream& in) {
    consume(in, 'u');
    consume(in, 'n');
    skipWhitespace(in);
    consume(in, '(');
    skipWhitespace(in);
    PTR(VarExpr) var = CAST(VarExpr)(parseVar(in));
    skipWhitespace(in);
    consume(in, ')');
    skipWhitespace(in);
    PTR(Expr) body = parseExpr(in);
    return NEW(_funExpr)(var->str, body);
}
