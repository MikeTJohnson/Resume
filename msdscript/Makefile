OBJS = main.o cmdLine.o Expr.o Tests.o parser.o Val.o Env.o
TESTOBJS = exec.o testmsdscript.o
TESTHEADERS = exec.h
CFLAGS = c++ -fsanitize=undefined -fno-sanitize-recover=undefined -std=c++17
HEADERS = Expr.hpp cmdLine.hpp catch.h parser.hpp Val.hpp Env.hpp

msdscript: $(OBJS) $(HEADERS)
	$(CFLAGS) $(OBJS) -o msdscript

main.o: main.cpp
	$(CFLAGS) -c main.cpp

cmdLine.o: cmdLine.hpp cmdLine.cpp
	$(CFLAGS) -c cmdLine.cpp

Expr.o: Expr.hpp Expr.cpp
	$(CFLAGS) -c Expr.cpp

Tests.o: catch.h
	$(CFLAGS) -c Tests.cpp

parser.o: parser.hpp parser.cpp
	$(CFLAGS) -c parser.cpp

Val.o: Val.hpp Val.cpp
	$(CFLAGS) -c Val.cpp

Env.o: Env.hpp Env.cpp
	$(CFLAGS) -c Env.cpp

clean: 
	rm -rf msdscript $(OBJS)

testmsdscript: $(TESTOBJS) $(TESTHEADERS)
	$(CFLAGS) $(TESTOBJS) -o testmsdscript

testmsdscript.o: testmsdscript.cpp
	$(CFLAGS) -c testmsdscript.cpp

exec.o: exec.cpp exec.h
	$(CFLAGS) -c exec.cpp

cleantest: $(TESTOBJS) testmsdscript
	rm -rf testmsdscript $(TESTOBJS)

.PHONY: test
test: msdscript
	./msdscript --test

.PHONY: doc
doc:
	cd documentation && doxygen

.PHONY: showdoc
showdoc:
	cd documentation/html && open index.html
