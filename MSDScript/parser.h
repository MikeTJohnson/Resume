#pragma once
#include <istream>
#include "pointers.h"
/**
 *\file parser.hpp
 *\brief A tool to parse expressions written by the user into expressions that are usable by the script
 *
 *Takes user written mathematical expressions and tokenizes them. Once the user input has been tokenized, the tokens are placed into expression objects and sent through the respective interp methods
 *\author Michael Johnson
 */

/**
 *\brief Simple declaration to use value objects in class methods
 */
class Expr;

extern PTR(Expr) parseExpr(std::istream &in);
extern void skipWhitespace(std::istream &in);
extern void consume(std::istream& in, int expect);
extern PTR(Expr) parseNum(std::istream &in);
extern PTR(Expr) parseAddend(std::istream &in);
extern PTR(Expr) parseVar(std::istream &in);
extern PTR(Expr) parseMulticand(std::istream &in);
extern PTR(Expr) parseKeyword(std::istream& in);
extern PTR(Expr) parseLet(std::istream& in);
extern PTR(Expr) parseTrue(std::istream& in);
extern PTR(Expr) parseFalse(std::istream& in);
extern PTR(Expr) parseIf(std::istream& in);
extern PTR(Expr) parseComparg(std::istream& in);
extern PTR(Expr) parseInner(std::istream& in);
extern PTR(Expr) parseFun(std::istream& in);
