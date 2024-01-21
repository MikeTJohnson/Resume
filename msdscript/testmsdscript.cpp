//
//  main.cpp
//  testMsdscript
//
//  Created by Michael Johnson on 2/24/23.
//

#include <iostream>
#include "exec.h"

static std::string randomExpr();

int main(int argc, const char * argv[]) {
    srand(clock());
    int iterations = 100;
    for (int i = 0; i < iterations; i++) {
        std::string expr = randomExpr();
        std::cout << "trying: " << expr << std::endl;
        if (argc == 2) {
            const char * const interp_argv[] = { argv[1], "--interp" };
            const char * const print_argv[] = { argv[1], "--print" };
            ExecResult interp_result = exec_program(2, interp_argv, expr);
            ExecResult print_result = exec_program(2, print_argv, expr);
            ExecResult interp_again_result = exec_program(2, interp_argv, print_result.out);
            std::cout << interp_result.out << std::endl;
            std::cout << interp_again_result.out << std::endl;
            if (interp_again_result.out != interp_result.out) {
                throw std::runtime_error("different result for printed");
            }
        }
        else if (argc == 3) {
            const char * const interp1_argv[] = { argv[1], "--interp" };
            const char * const interp2_argv[] = { argv[2], "--interp" };
            ExecResult interp1_result = exec_program(2, interp1_argv, expr);
            ExecResult interp2_result = exec_program(2, interp2_argv, expr);
            if (interp1_result.out != interp2_result.out) {
                std::cout << "1st " << interp1_result.out;
                std::cout << "2nd " << interp2_result.out;
                throw std::runtime_error("different results");
            }
        }
        else {
            throw std::runtime_error("too many arguments");
        }
    }
    std::cout << "automated testing passed" << std::endl;
    
    return 0;
}

static std::string randomExpr() {
    int check = rand()%100;
    int numProb = 60;
    int negativeProb = 50;
    int addProb = 78;
    int multProb = 94;
    int yVarProb = 98;
    int xVarProb = 96;
    int xSubProb = 99;
    if (check < numProb) {
        int internalCheck = rand()%100;
        if (internalCheck < negativeProb) {
            return std::to_string(0 - rand()%100);
        }
        else {
            return std::to_string(rand()%100);
        }
    }
    else if (check <= addProb) {
        return randomExpr() + " + " + randomExpr();
    }
    else if (check <= multProb) {
        return randomExpr() + " * " + randomExpr();
    }
    else if (check <= xVarProb) {
        return "x";
    }
    else if (check <= xSubProb) {
        return "_let x=" + randomExpr() + " _in x + " + randomExpr();
    }
    else {
        return "_let x=" + randomExpr() + " _in " + randomExpr();
    }
    return 0;
}
