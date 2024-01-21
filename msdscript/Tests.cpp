//
//  Tests.cpp
//  msdscript
//
//  Created by Michael Johnson on 1/27/23.
//

#include <stdio.h>
#include "catch.h"
#include "Val.hpp"
#include "Expr.hpp"
#include "pointers.h"
#include "Env.hpp"

TEST_CASE("testNumEquals") {
    //22 == 22
    CHECK ((NEW(NumExpr)(22))->equals(NEW(NumExpr)(22)) == true);
    
    //-1 == -1
    CHECK ((NEW(NumExpr)(-1))->equals(NEW(NumExpr)(-1)) == true);
    
    //0 == 0
    CHECK ((NEW(NumExpr)(0))->equals(NEW(NumExpr)(0)) == true);
    
    //1 != 8
    CHECK((NEW(NumExpr)(1))->equals(NEW(NumExpr)(8)) == false);
    
    //1000000 == 1000000
    CHECK ((NEW(NumExpr)(1000000))->equals(NEW(NumExpr)(1000000)) == true);
    
    //-1000000 == -1000000
    CHECK ((NEW(NumExpr)(-1000000))->equals(NEW(NumExpr)(-1000000)) == true);
    
    //1 == 1
    CHECK ((NEW(NumExpr)(1))->equals(NEW(NumExpr)(1)) == true);
}


TEST_CASE("testNumInterp") {
    CHECK((NEW(NumExpr)(22))->interp(NEW (EmptyEnv)())->equals(NEW(NumVal)(22)));
    CHECK((NEW(NumExpr)(0))->interp(NEW (EmptyEnv)()) ->equals(NEW(NumVal)(0)));
    CHECK((NEW(NumExpr)(1))->interp(NEW (EmptyEnv)())->equals(NEW(NumVal)(1)));
    CHECK((NEW(NumExpr)(-1))->interp(NEW (EmptyEnv)())->equals(NEW(NumVal)(-1)));
    CHECK((NEW(NumExpr)(1000000))->interp(NEW (EmptyEnv)())->equals(NEW(NumVal)(1000000)));
    CHECK((NEW(NumExpr)(-1000000))->interp(NEW (EmptyEnv)())->equals(NEW(NumVal)(-1000000)));
    CHECK((NEW(NumExpr)(22))->interp(NEW (EmptyEnv)())->equals(NEW(NumVal)(-22)) == false);
}


TEST_CASE("testMultEquals") {
    //22*22 == 22*22
    CHECK((NEW(MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(22)))->equals(NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(22))) == true);
    
    //0*0 == 0*0
    CHECK((NEW (MultExpr)(NEW (NumExpr)(0), NEW (NumExpr)(0)))->equals(NEW (MultExpr)(NEW (NumExpr)(0), NEW (NumExpr)(0))) == true);
    
    //-1*1 == -1*1
    CHECK((NEW (MultExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1)))->equals(NEW (MultExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1))) == true);
    
    //1*-1 != -1*1
    CHECK((NEW (MultExpr)(NEW (NumExpr)(1), NEW (NumExpr)(-1)))->equals(NEW (MultExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1))) == false);
    
    //(22*(22*1)) == (22*(22*1))
    CHECK((NEW (MultExpr)(NEW (NumExpr)(22), NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(1))))->equals(NEW (MultExpr)(NEW (NumExpr)(22), NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(1)))) == true);
    
    //(22*(22*1)) != (22*(1*22))
    CHECK((NEW (MultExpr)(NEW (NumExpr)(22), NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(1))))->equals(NEW (MultExpr)(NEW (NumExpr)(22), NEW (MultExpr)(NEW (NumExpr)(1), NEW (NumExpr)(22)))) == false);
    
    //(22*0)*(-1*1) == (22*0)*(-1*1)
    CHECK((NEW (MultExpr)(NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)), NEW (MultExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1))))->equals(NEW (MultExpr)(NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)), NEW (MultExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1)))) == true);
    
    //(22*0)*(-1*1) != (22*0)*(1*-1)
    CHECK((NEW (MultExpr)(NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)), NEW (MultExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1))))->equals(NEW (MultExpr)(NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)), NEW (MultExpr)(NEW (NumExpr)(1), NEW (NumExpr)(-1)))) == false);
    
    //0*1 != 0
    CHECK((NEW (MultExpr)(NEW (NumExpr)(0), NEW (NumExpr)(1)))->equals(NEW (NumExpr)(0)) == false);
    
    //22*0 != 22+0
    CHECK((NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)))->equals(NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0))) == false);
    
    //22*0 != -1+1
    CHECK((NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)))->equals(NEW (AddExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1))) == false);
    
    //1000000*22 == 1000000*22
    CHECK((NEW (MultExpr)(NEW (NumExpr)(1000000), NEW (NumExpr)(22)))->equals(NEW (MultExpr)(NEW (NumExpr)(1000000), NEW (NumExpr)(22))) == true);
    
    //-1000000*22 == -1000000*22
    CHECK((NEW (MultExpr)(NEW (NumExpr)(-1000000), NEW (NumExpr)(22)))->equals(NEW (MultExpr)(NEW (NumExpr)(-1000000), NEW (NumExpr)(22))) == true);
}

TEST_CASE("testMultInterp") {
    //22*22 == 484
    CHECK((NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(22)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(484)));
    
    //0*0 == 0
    CHECK((NEW (MultExpr)(NEW (NumExpr)(0), NEW (NumExpr)(0)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(0)));
    
    //-1*1 == -1
    CHECK((NEW (MultExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(-1)));
    
    //1*-1 != 1
    CHECK((NEW (MultExpr)(NEW (NumExpr)(1), NEW (NumExpr)(-1)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(1)) == false);
    
    //(22*(22*1)) == 484
    CHECK((NEW (MultExpr)(NEW (NumExpr)(22), NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(1))))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(484)));
    
    //(1*(22*22)) == 484
    CHECK((NEW (MultExpr)(NEW (NumExpr)(1), NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(22))))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(484)));
    
    //(22*0)*(-1*1) == 0
    CHECK((NEW (MultExpr)(NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)), NEW (MultExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1))))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(0)));
    
    //(22*1)*(-1*1) == -22
    CHECK((NEW (MultExpr)(NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(1)), NEW (MultExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1))))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(-22)));
    
    //1*1 == 1
    CHECK((NEW (MultExpr)(NEW (NumExpr)(1), NEW (NumExpr)(1)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(1)));
    
    //-1*(-1) == 1
    CHECK((NEW (MultExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(-1)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(1)));
    
    //1000000*22 == 22000000
    CHECK((NEW (MultExpr)(NEW (NumExpr)(1000000), NEW (NumExpr)(22)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(22000000)));
    
    //-1000000*22 == -22000000
    CHECK((NEW (MultExpr)(NEW (NumExpr)(-1000000), NEW (NumExpr)(22)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(-22000000)));

}

TEST_CASE("testAddEquals") {
    //22+22 == 22+22
    CHECK((NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(22)))->equals(NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(22))) == true);
    
    //0+0 == 0+0
    CHECK((NEW (AddExpr)(NEW (NumExpr)(0), NEW (NumExpr)(0)))->equals(NEW (AddExpr)(NEW (NumExpr)(0), NEW (NumExpr)(0))) == true);
    
    //-1+1 == -1+1
    CHECK((NEW (AddExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1)))->equals(NEW (AddExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1))) == true);
    
    //1+(-1) != -1+1
    CHECK((NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(-1)))->equals(NEW (AddExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1))) == false);
    
    //22+(22+1) == 22+(22+1)
    CHECK((NEW (AddExpr)(NEW (NumExpr)(22), NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(1))))->equals(NEW (AddExpr)(NEW (NumExpr)(22), NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(1)))) == true);
    
    //22+(22+1) != 22+(1+22)
    CHECK((NEW (AddExpr)(NEW (NumExpr)(22), NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(1))))->equals(NEW (AddExpr)(NEW (NumExpr)(22), NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(22)))) == false);
    
    //(22+0)+(-1+1) == (22+0)+(-1+1)
    CHECK((NEW (AddExpr)(NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)), NEW (AddExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1))))->equals(NEW (AddExpr)(NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)), NEW (AddExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1)))) == true);
    
    //(22+0)+(-1+1) != (22+0)+(1+(-1))
    CHECK((NEW (AddExpr)(NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)), NEW (AddExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1))))->equals(NEW (AddExpr)(NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)), NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(-1)))) == false);
    
    //-1+1 != 0
    CHECK((NEW (AddExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1)))->equals(NEW (NumExpr)(0)) == false);
    
    //22+0 != 22*0
    CHECK((NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)))->equals(NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0))) == false);
    
    //-1+1 != 22*0
    CHECK((NEW (AddExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1)))->equals(NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0))) == false);
    
    //1000000+22 == 1000000+22
    CHECK((NEW (AddExpr)(NEW (NumExpr)(1000000), NEW (NumExpr)(22)))->equals(NEW (AddExpr)(NEW (NumExpr)(1000000), NEW (NumExpr)(22))) == true);
    
    //-1000000+22 == -1000000+22
    CHECK((NEW (AddExpr)(NEW (NumExpr)(-1000000), NEW (NumExpr)(22)))->equals(NEW (AddExpr)(NEW (NumExpr)(-1000000), NEW (NumExpr)(22))) == true);
}

TEST_CASE("testAddInterp") {
    //22+22 == 44
    CHECK((NEW (AddExpr)( NEW (NumExpr)(22), NEW (NumExpr)(22)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(44)));
    
    //1+1 == 2
    CHECK((NEW (AddExpr)( NEW (NumExpr)(1), NEW (NumExpr)(1)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(2)));
    
    //1+1 != 1
    CHECK((NEW (AddExpr)( NEW (NumExpr)(1), NEW (NumExpr)(1)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(1)) == false);
    
    //1+0 == 1
    CHECK((NEW (AddExpr)( NEW (NumExpr)(1), NEW (NumExpr)(0)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(1)));
    
    //0+1 == 1
    CHECK((NEW (AddExpr)( NEW (NumExpr)(0), NEW (NumExpr)(1)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(1)));
    
    //0+0 == 0
    CHECK((NEW (AddExpr)( NEW (NumExpr)(0), NEW (NumExpr)(0)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(0)));
    
    //-1+(-1) == -2
    CHECK((NEW (AddExpr)( NEW (NumExpr)(-1), NEW (NumExpr)(-1)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(-2)));
    
    //-1+1 == 0
    CHECK((NEW (AddExpr)( NEW (NumExpr)(-1), NEW (NumExpr)(1)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(0)));
    
    //-1+1 != 2
    CHECK((NEW (AddExpr)( NEW (NumExpr)(-1), NEW (NumExpr)(1)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(2)) == false);
    
    //1000000+1000000 == 2000000
    CHECK((NEW (AddExpr)( NEW (NumExpr)(1000000), NEW (NumExpr)(1000000)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(2000000)));
    
    //-1000000+(-1000000) == -2000000
    CHECK((NEW (AddExpr)( NEW (NumExpr)(-1000000), NEW (NumExpr)(-1000000)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(-2000000)));
    
    //(1+1)+(1+1) == 4
    CHECK((NEW (AddExpr)( NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(1)), NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(1))))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(4)));
}

TEST_CASE("testVarEquals") {
    //a == a
    CHECK ((NEW (VarExpr)("a"))->equals(NEW (VarExpr)("a")) == true);
    
    //A == A
    CHECK ((NEW (VarExpr)("A"))->equals(NEW (VarExpr)("A")) == true);
    
    //1 == 1
    CHECK ((NEW (VarExpr)("1"))->equals(NEW (VarExpr)("1")) == true);
    
    //a != A
    CHECK((NEW (VarExpr)("a"))->equals(NEW (VarExpr)("A")) == false);
    
    //whyNotGoToPandaExpressAfterAll == whyNotGoToPandaExpressAfterAll
    CHECK ((NEW (VarExpr)("whyNotGoToPandaExpressAfterAll"))->equals(NEW (VarExpr)("whyNotGoToPandaExpressAfterAll")) == true);
    
    //-1000000 == -1000000
    CHECK ((NEW (VarExpr)("-1000000"))->equals(NEW (VarExpr)("-1000000")) == true);
    
    //"1" != 1
    CHECK((NEW (VarExpr)("1"))->equals(NEW (NumExpr)(1)) == false);
}

TEST_CASE("testVarInterp") {
    //make sure it throws
    CHECK_THROWS_WITH((NEW (VarExpr)("x"))->interp(NEW (EmptyEnv)()), "free variable: x");
}

TEST_CASE("testVarHasVariable") {
    //varaibles are variables
    CHECK((NEW (VarExpr)("x"))->hasVariable() == true);
    CHECK((NEW (VarExpr)("hello"))->hasVariable() == true);
    CHECK((NEW (VarExpr)("whyNotGoToPandaExpressAfterAll"))->hasVariable() == true);
}

TEST_CASE("testNumHasVariable") {
    //numbers aren't variables
    CHECK((NEW (NumExpr)(0))->hasVariable() == false);
    CHECK((NEW (NumExpr)(1))->hasVariable() == false);
    CHECK((NEW (NumExpr)(-1))->hasVariable() == false);
    CHECK((NEW (NumExpr)(1000000))->hasVariable() == false);
    CHECK((NEW (NumExpr)(-1000000))->hasVariable() == false);
}

TEST_CASE("testAddHasVariable") {
    //num + num
    CHECK((NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2)))->hasVariable() == false);
    
    //num + var
    CHECK((NEW (AddExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x")))->hasVariable() == true);
    
    //var + num
    CHECK((NEW (AddExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(1)))->hasVariable() == true);
    
    //var + var
    CHECK((NEW (AddExpr)(NEW (VarExpr)("x"), NEW (VarExpr)("y")))->hasVariable() == true);
    
    //novar + novar
    CHECK((NEW (AddExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2)), (NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2)))))->hasVariable() == false);
    
    //hasvar + novar
    CHECK((NEW (AddExpr)(NEW (AddExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(2)), (NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2)))))->hasVariable() == true);
    
    //novar + hasvar
    CHECK((NEW (AddExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2)), (NEW (AddExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(2)))))->hasVariable() == true);
    
    //hasvar + hasvar
    CHECK((NEW (AddExpr)(NEW (AddExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(2)), (NEW (AddExpr)(NEW (VarExpr)("y"), NEW (NumExpr)(2)))))->hasVariable() == true);
}

TEST_CASE("testMultHasVariable") {
    //num * num
    CHECK((NEW (MultExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2)))->hasVariable() == false);

    //num * var
    CHECK((NEW (MultExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x")))->hasVariable() == true);

    //var * num
    CHECK((NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(1)))->hasVariable() == true);

    //var * var
    CHECK((NEW (MultExpr)(NEW (VarExpr)("x"), NEW (VarExpr)("y")))->hasVariable() == true);
    
    //novar * novar
    CHECK((NEW (MultExpr)(NEW (MultExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2)), (NEW (MultExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2)))))->hasVariable() == false);
    
    //hasvar * novar
    CHECK((NEW (MultExpr)(NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(2)), (NEW (MultExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2)))))->hasVariable() == true);
    
    //novar * hasvar
    CHECK((NEW (MultExpr)(NEW (MultExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2)), (NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(2)))))->hasVariable() == true);
    
    //hasvar * hasvar
    CHECK((NEW (MultExpr)(NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(2)), (NEW (MultExpr)(NEW (VarExpr)("y"), NEW (NumExpr)(2)))))->hasVariable() == true);
}

TEST_CASE("testNumToString") {
    //nums go to strings correctly
    CHECK((NEW (NumExpr)(-1))->toString() == "-1");
    CHECK((NEW (NumExpr)(-1))->toString() != "1");
    CHECK((NEW (NumExpr)(0))->toString() == "0");
    CHECK((NEW (NumExpr)(1))->toString() == "1");
    CHECK((NEW (NumExpr)(1000000))->toString() == "1000000");
    CHECK((NEW (NumExpr)(-1000000))->toString() == "-1000000");
}

TEST_CASE("testVarToString") {
    //vars go to strings correctly
    CHECK((NEW (VarExpr)("a"))->toString() == "a");
    CHECK((NEW (VarExpr)("A"))->toString() == "A");
    CHECK((NEW (VarExpr)("1"))->toString() == "1");
    CHECK((NEW (VarExpr)("a"))->toString() != "A");
    CHECK((NEW (VarExpr)("whyNotGoToPandaExpressAfterAll"))->toString() == "whyNotGoToPandaExpressAfterAll");
    CHECK((NEW (VarExpr)("-1000000"))->toString() == "-1000000");
}

TEST_CASE("testAddToString") {
    //22+22 == (22+22)
    CHECK((NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(22)))->toString() == "(22+22)");
    
    //0+0 == (0+0)
    CHECK((NEW (AddExpr)(NEW (NumExpr)(0), NEW (NumExpr)(0)))->toString() == "(0+0)");
    
    //-1+1 == (-1+1)
    CHECK((NEW (AddExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1)))->toString() == "(-1+1)");
    
    //1+(-1) != (-1+1)
    CHECK((NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(-1)))->toString() != "(-1+1)");
    
    //22+(22+1) == (22+(22+1))
    CHECK((NEW (AddExpr)(NEW (NumExpr)(22), NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(1))))->toString() == "(22+(22+1))");
    
    //22+(22+1) != 22+(1+22)
    CHECK((NEW (AddExpr)(NEW (NumExpr)(22), NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(1))))->toString() != "(22+(1+22))");
    
    //(22+0)+(-1+1) == ((22+0)+(-1+1))
    CHECK((NEW (AddExpr)(NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)), NEW (AddExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1))))->toString() == "((22+0)+(-1+1))");
    
    //(22+0)+(-1+1) != ((22+0)+(1+-1))
    CHECK((NEW (AddExpr)(NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)), NEW (AddExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1))))->toString() != "((22+0)+(1+-1))");
    
    //-1+1 != (0)
    CHECK((NEW (AddExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1)))->toString() != "(0)");
    
    //22+0 != 22*0
    CHECK((NEW (AddExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)))->toString() != "(22*0)");
    
    //1000000+22 == (1000000+22)
    CHECK((NEW (AddExpr)(NEW (NumExpr)(1000000), NEW (NumExpr)(22)))->toString() == "(1000000+22)");
    
    //-1000000+22 == (-1000000+22)
    CHECK((NEW (AddExpr)(NEW (NumExpr)(-1000000), NEW (NumExpr)(22)))->toString() == "(-1000000+22)");
}

TEST_CASE("testMultToString") {
    //22*22 == (22*22)
    CHECK((NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(22)))->toString() == "(22*22)");
    
    //0*0 == (0*0)
    CHECK((NEW (MultExpr)(NEW (NumExpr)(0), NEW (NumExpr)(0)))->toString() == "(0*0)");
    
    //-1*1 == (-1*1)
    CHECK((NEW (MultExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1)))->toString() == "(-1*1)");
    
    //1*-1 != (-1*1)
    CHECK((NEW (MultExpr)(NEW (NumExpr)(1), NEW (NumExpr)(-1)))->toString() != "(-1*1)");
    
    //(22*(22*1)) == (22*(22*1))
    CHECK((NEW (MultExpr)(NEW (NumExpr)(22), NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(1))))->toString() == "(22*(22*1))");
    
    //(22*(22*1)) != (22*(1*22))
    CHECK((NEW (MultExpr)(NEW (NumExpr)(22), NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(1))))->toString() != "(22*(1*22))");
    
    //(22*0)*(-1*1) == ((22*0)*(-1*1))
    CHECK((NEW (MultExpr)(NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)), NEW (MultExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1))))->toString() == "((22*0)*(-1*1))");
    
    //(22*0)*(-1*1) != ((22*0)*(1*-1))
    CHECK((NEW (MultExpr)(NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)), NEW (MultExpr)(NEW (NumExpr)(-1), NEW (NumExpr)(1))))->toString() != "((22*0)*(1*-1))");
    
    //0*1 != (0)
    CHECK((NEW (MultExpr)(NEW (NumExpr)(0), NEW (NumExpr)(1)))->toString() != "(0)");
    
    //22*0 != (22+0)
    CHECK((NEW (MultExpr)(NEW (NumExpr)(22), NEW (NumExpr)(0)))->toString() != "(22+0)");
    
    //1000000*22 == (1000000*22)
    CHECK((NEW (MultExpr)(NEW (NumExpr)(1000000), NEW (NumExpr)(22)))->toString() == "(1000000*22)");
    
    //-1000000*22 == (-1000000*22)
    CHECK((NEW (MultExpr)(NEW (NumExpr)(-1000000), NEW (NumExpr)(22)))->toString() == "(-1000000*22)");
}

TEST_CASE("testAddPrettyPrintAs") {
    std::stringstream out("");
    (NEW (AddExpr)(NEW (NumExpr)(1), NEW (MultExpr)(NEW (NumExpr)(2), NEW (NumExpr)(3))))->prettyPrintAs(out) ;
    CHECK(out.str() == "1 + 2 * 3");
}

TEST_CASE("testMultPrettyPrintAs") {
    std::stringstream out("");

    (NEW (MultExpr)(NEW (NumExpr)(1), NEW (AddExpr)(NEW (NumExpr)(2), NEW (NumExpr)(3))))->prettyPrintAs(out) ;
    CHECK(out.str() == "1 * (2 + 3)");
    
    out.str(std::string());
    (NEW (MultExpr)(NEW (NumExpr)(2), NEW (MultExpr)(NEW (NumExpr)(3), NEW (NumExpr)(4))))->prettyPrintAs(out) ;
    CHECK(out.str() == "2 * 3 * 4");
}

TEST_CASE("testLetEquals") {
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (NumExpr)(2)))->equals(NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (NumExpr)(2))) == true);
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (NumExpr)(2)))->equals(NEW (_letExpr)("x", NEW (NumExpr)(2), NEW (NumExpr)(1))) == false);
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (NumExpr)(2)))->equals(NEW (_letExpr)("y", NEW (NumExpr)(1), NEW (NumExpr)(2))) == false);
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (NumExpr)(2)))->equals(NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (AddExpr)(NEW (NumExpr)(2), NEW (NumExpr)(1)))) == false);
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (AddExpr)(NEW (NumExpr)(2), NEW (NumExpr)(1))))->equals(NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (AddExpr)(NEW (NumExpr)(2), NEW (NumExpr)(1)))) == true);
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (AddExpr)(NEW (NumExpr)(2), NEW (NumExpr)(1))))->equals(NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2)))) == false);
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (AddExpr)(NEW (NumExpr)(2), NEW (NumExpr)(1))))->equals(NEW (_letExpr)("x", NEW (AddExpr)(NEW (NumExpr)(2), NEW (NumExpr)(1)), NEW (NumExpr)(1))) == false);
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (AddExpr)(NEW (NumExpr)(2), NEW (NumExpr)(1))))->equals(NEW (_letExpr)("x", NEW (VarExpr)("y"), NEW (VarExpr)("z"))) == false);
    
    CHECK((NEW (_letExpr)("x", NEW (VarExpr)("y"), NEW (VarExpr)("z")))->equals(NEW (_letExpr)("x", NEW (VarExpr)("y"), NEW (VarExpr)("z"))) == true);
    
    CHECK((NEW (_letExpr)("x", NEW (VarExpr)("y"), NEW (VarExpr)("z")))->equals(NEW (_letExpr)("x", NEW (NumExpr)(0), NEW (MultExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x")), NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(2))))) == false);
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(0), NEW (MultExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x")), NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(2)))))->equals(NEW (_letExpr)("x", NEW (NumExpr)(0), NEW (MultExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x")), NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(2))))) == true);
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(0), NEW (MultExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x")), NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(2)))))->equals(NEW (_letExpr)("x", NEW (NumExpr)(0), NEW (MultExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x")), NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(3))))) == false);
}

TEST_CASE("testLetInterp") {
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (NumExpr)(2)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(2)));
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(2), NEW (NumExpr)(1)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(1)));
    
    CHECK((NEW (_letExpr)("y", NEW (NumExpr)(1), NEW (NumExpr)(2)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(2)));

    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(0), NEW (MultExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x")), NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(2)))))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(0)));
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(2), NEW (MultExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x")), NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(3)))))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(18)));
    
    CHECK_THROWS_WITH((NEW (_letExpr)("x", NEW (VarExpr)("y"), NEW (MultExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x")), NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(3)))))->interp(NEW (EmptyEnv)()), "free variable: y");
}

TEST_CASE("testLetHasVariable") {
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (NumExpr)(2)))->hasVariable() == false);
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(2), NEW (NumExpr)(1)))->hasVariable() == false);
    
    CHECK((NEW (_letExpr)("y", NEW (NumExpr)(1), NEW (NumExpr)(2)))->hasVariable() == false);

    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (AddExpr)(NEW (NumExpr)(2), NEW (NumExpr)(1))))->hasVariable() == false);
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2))))->hasVariable() == false);

    CHECK((NEW (_letExpr)("x", NEW (AddExpr)(NEW (NumExpr)(2), NEW (NumExpr)(1)), NEW (NumExpr)(1)))->hasVariable() == false);
    
    CHECK((NEW (_letExpr)("x", NEW (VarExpr)("y"), NEW (VarExpr)("z")))->hasVariable() == true);

    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(0), NEW (MultExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x")), NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(2)))))->hasVariable() == true);
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(0), NEW (MultExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x")), NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(3)))))->hasVariable() == true);
}

TEST_CASE("testLetToString") {
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(1), NEW (NumExpr)(2)))->toString() == "(_let x=1 _in 2)");
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(2), NEW (NumExpr)(1)))->toString() == "(_let x=2 _in 1)");
    
    CHECK((NEW (_letExpr)("y", NEW (NumExpr)(1), NEW (NumExpr)(2)))->toString() == "(_let y=1 _in 2)");

    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(0), NEW (MultExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x")), NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(2)))))->toString() == "(_let x=0 _in ((1+x)*(x*2)))");
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(0), NEW (MultExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x")), NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(3)))))->toString() == "(_let x=0 _in ((1+x)*(x*3)))");
    
    CHECK((NEW (_letExpr)("y", NEW (NumExpr)(0), NEW (MultExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x")), NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(2)))))->toString() == "(_let y=0 _in ((1+x)*(x*2)))");
    
    CHECK((NEW (_letExpr)("x", NEW (VarExpr)("y"), NEW (MultExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x")), NEW (MultExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(3)))))->toString() == "(_let x=y _in ((1+x)*(x*3)))");
    
    CHECK((NEW (_letExpr)("x", NEW (NumExpr)(5), NEW (AddExpr)(NEW (_letExpr)("y", NEW (NumExpr)(3), NEW (AddExpr)(NEW (VarExpr)("y"), NEW (NumExpr)(2))), NEW (VarExpr)("x"))))->toString() == "(_let x=5 _in ((_let y=3 _in (y+2))+x))");
}

TEST_CASE("Pretty Print examples_Kevin"){           //Created from assignment examples
    std::stringstream out("");
    (NEW (_letExpr)("x", NEW (NumExpr)(5), NEW (AddExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(1))))->prettyPrintAs(out);
    CHECK(out.str() == "_let x = 5\n_in  x + 1");

    out.str(std::string());
    (NEW (AddExpr)(NEW (_letExpr)("x", NEW (NumExpr)(5), NEW (VarExpr)("x")), NEW (NumExpr)(1)))->prettyPrintAs(out) ;
    CHECK(out.str() == "(_let x = 5\n"
                        " _in  x) + 1");
    
    out.str(std::string());

    (NEW (MultExpr)(NEW (NumExpr)(5), NEW (_letExpr)("x", NEW (NumExpr)(5), NEW (AddExpr)(NEW (VarExpr)("x"), NEW (NumExpr)(1)))))->prettyPrintAs(out);
    CHECK(out.str() == "5 * _let x = 5\n"
                       "    _in  x + 1");
    
    out.str(std::string());

    (NEW (AddExpr)(NEW (MultExpr)(NEW (NumExpr)(5), NEW (_letExpr)("x", NEW (NumExpr)(5), NEW (VarExpr)("x"))), NEW (NumExpr)(1)))->prettyPrintAs(out) ;
    CHECK(out.str() == "5 * (_let x = 5\n"
                       "     _in  x) + 1");

    out.str(std::string());

    (NEW (_letExpr)("x", NEW (NumExpr)(5), NEW (AddExpr)(NEW (_letExpr)("y", NEW (NumExpr)(3), NEW (AddExpr)(NEW (VarExpr)("y"), NEW (NumExpr)(2))), NEW (VarExpr)("x"))))->prettyPrintAs(out) ;
    CHECK(out.str() == "_let x = 5\n"
                        "_in  (_let y = 3\n"
                        "      _in  y + 2) + x");
}

TEST_CASE("testNumValEquals") {
    CHECK((NEW (NumVal)(0))->equals(NEW (NumVal)(0)));
    CHECK((NEW (NumVal)(1))->equals(NEW (NumVal)(1)));
    CHECK((NEW (NumVal)(-1))->equals(NEW (NumVal)(-1)));
    CHECK((NEW (NumVal)(1))->equals(NEW (NumVal)(0)) == false);
}

TEST_CASE("testConditionalsFromSlides") {
    CHECK((NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2)))->interp(NEW (EmptyEnv)())
    ->equals(NEW (NumVal)(3)));
    
    CHECK((NEW (EqExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2)))->interp(NEW (EmptyEnv)())
    ->equals(NEW (BoolVal)(false)));

    CHECK((NEW (EqExpr)(NEW (NumExpr)(2), NEW (NumExpr)(2)))->interp(NEW (EmptyEnv)())
    ->equals(NEW (BoolVal)(true)));

    CHECK((NEW (EqExpr)(NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(1)), NEW (NumExpr)(2)))->interp(NEW (EmptyEnv)())
    ->equals(NEW (BoolVal)(true)));
    
    CHECK((NEW (EqExpr)(NEW (NumExpr)(2), NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(1))))->interp(NEW (EmptyEnv)())
    ->equals(NEW (BoolVal)(true)));
    
    CHECK((NEW (_letExpr)("x",
    NEW (AddExpr)(NEW (NumExpr)(2), NEW (NumExpr)(3)),
    NEW (MultExpr)(NEW (VarExpr)("x"), NEW (VarExpr)("x"))))
    ->interp(NEW (EmptyEnv)())
    ->equals(NEW (NumVal)(25)));
    
    CHECK((NEW (_ifExpr)(NEW (BoolExpr)(true),
    NEW (NumExpr)(1),
    NEW (NumExpr)(2)))->interp(NEW (EmptyEnv)())
    ->equals(NEW (NumVal)(1)));
}

TEST_CASE("testFunExprEquals") {
    CHECK((NEW (_funExpr)("x", NEW (NumExpr)(2)))->equals(NEW (_funExpr)("x", NEW (NumExpr)(2))));
    
    CHECK((NEW (_funExpr)("x", NEW (NumExpr)(2)))->equals(NEW (_funExpr)("x", NEW (NumExpr)(1))) == false);
    
    CHECK((NEW (_funExpr)("x", NEW (NumExpr)(2)))->equals(NEW (NumExpr)(1)) == false);
}

TEST_CASE("testFunExprHasVariable") {
    CHECK((NEW (_funExpr)("x", NEW (NumExpr)(2)))->hasVariable() == false);
    
    CHECK((NEW (_funExpr)("x", NEW (VarExpr)("x")))->hasVariable() == true);
    
    CHECK((NEW (_funExpr)("x", NEW (AddExpr)(NEW (NumExpr)(2), NEW (VarExpr)("x"))))->hasVariable() == true);
}

TEST_CASE("testFunToString") {
    CHECK((NEW (_funExpr)("x", NEW (NumExpr)(1)))->toString() == ("(fun(x) 1)"));
}

TEST_CASE("testCallEquals") {
    CHECK((NEW (callExpr)(NEW (_funExpr)("x", NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2))), NEW (VarExpr)("x")))->equals(NEW (callExpr)(NEW (_funExpr)("x", NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2))), NEW (VarExpr)("x"))));
    
    CHECK((NEW (callExpr)(NEW (_funExpr)("x", NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2))), NEW (VarExpr)("x")))->equals(NEW (callExpr)(NEW (_funExpr)("x", NEW (AddExpr)(NEW (NumExpr)(2), NEW (NumExpr)(2))), NEW (VarExpr)("x"))) == false);
}

TEST_CASE("testCallToString") {
    CHECK((NEW (callExpr)(NEW (_funExpr)("x", NEW (AddExpr)(NEW (NumExpr)(1), NEW (NumExpr)(2))), NEW (VarExpr)("x")))->toString() == "((fun(x) (1+2)))(x)");
}

TEST_CASE("testCallInterp") {
    CHECK((NEW (callExpr)(NEW (_funExpr)("x", NEW (AddExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x"))), NEW (NumExpr)(2)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(3)));
    
    CHECK((NEW (callExpr)(NEW (_funExpr)("x", NEW (MultExpr)(NEW (NumExpr)(1), NEW (VarExpr)("x"))), NEW (NumExpr)(2)))->interp(NEW (EmptyEnv)())->equals(NEW (NumVal)(2)));
}
