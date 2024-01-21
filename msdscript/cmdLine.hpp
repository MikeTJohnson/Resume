//
//  cmdLine.hpp
//  msdscript
//
//  Created by Michael Johnson on 1/12/23.
//

#pragma once
#include <stdio.h>

/**
 *\file cmdLine.hpp
 */


typedef enum{
    doError,
    doTest,
    doHelp,
    doNothing,
    doInterp,
    doPrint,
    doPrettyPrint
} runModeT;

/**
 *\brief Function that performs actions based on command line arguments. Recommend using "--help" for comprehensive list of valid arguments
 */

runModeT useArguments(int argc, char* argv[]);



