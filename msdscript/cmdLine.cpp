//
//  cmdLine.cpp
//  msdscript
//
//  Created by Michael Johnson on 1/12/23.
//

#include "cmdLine.hpp"
#include <iostream>
#include <string>

/**
 *\file cmdLine.cpp
 *\brief takes arguments from the command line and performs actions using those arguments. Run with argument "--help" for comprehensive list of valid arguments
 *\author Michael Johnson
 */

/**
 *\brief takes arguments from the command line and performs actions using those arguments. Run with argument "--help" for comprehensive list of valid arguments
 *\param argc Number of arguments being passed
 *\param *argv[] Command line arguments
 */
runModeT useArguments(int argc, char* argv[]) {
    if (argc > 1) {
        for (int i = 1; i < argc; i++) {
            std::string temp = argv[i];
            if (temp == "--help") {
                return doHelp;
            }
            else if (temp == "--print") {
                return doPrint;
            }
            else if (temp == "--prettyprint") {
                return doPrettyPrint;
            }
            else if (temp == "--interp") {
                return doInterp;
            }
            else if (temp == "--test") {
                return doTest;
            }
            else {
                return doError;
            }
        }
    }
    return doNothing;
}
