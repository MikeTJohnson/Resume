//
//  main.cpp
//  msdscript
//
//  Created by Michael Johnson on 1/12/23.
//

/**
 *\mainpage MSDscript
 *\author Michael Johnson
 *\date 1/12/2023
 */

#include <iostream>
#include "cmdLine.hpp"
#include "Expr.hpp"
#include "parser.hpp"
#define CATCH_CONFIG_RUNNER
#include "catch.h"
#include "Val.hpp"
#include "pointers.h"
#include "Env.hpp"


int main(int argc, char * argv[]) {
    try {
        PTR(Expr) n;
        int cmd = useArguments(argc, argv);
        if (cmd == doError) {
            std::cerr << "not a valid command";
            exit(1);
        }
        else if (cmd == doHelp) {
            std::cout << "Here are the commands you can enter:" << std::endl;
            std::cout << "--help" << std::endl;
            std::cout << "--test" << std::endl;
            std::cout << "--print" << std::endl;
            std::cout << "--prettyprint" << std::endl;
            std::cout << "--interp" << std::endl;
        }
        else if (cmd == doTest) {
            Catch::Session().run(1, argv);
        }
        else if (cmd == doInterp) {
            PTR(Expr) e = parseExpr(std::cin);
            PTR(EmptyEnv) env = NEW(EmptyEnv)();
            std::cout << e->interp(env)->toString() << std::endl;
        }
        else if (cmd == doPrint) {
            PTR(Expr) e = parseExpr(std::cin);
            std::cout << e->toString() << std::endl;
        }
        else if (cmd == doPrettyPrint) {
            PTR(Expr) e = parseExpr(std::cin);
            std::cout << e->toPrettyString() << std::endl;
        }
        return 0;
    }
    catch (std::runtime_error exn) {
        std::cerr << exn.what() << "\n";
        return 1;
    }
}
