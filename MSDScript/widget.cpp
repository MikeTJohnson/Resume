#include "widget.h"
#include "expr.h"
#include "parser.h"
#include "pointers.h"
#include "env.h"
#include "val.h"

Widget::Widget(QWidget *parent)
    : QWidget(parent)
{
    expression = new QLabel("Expression", this);
    result = new QLabel("Result", this);
    interpButton = new QRadioButton("Interp", this);
    printButton = new QRadioButton("Print", this);
    submit = new QPushButton("submit", this);
    reset = new QPushButton("reset", this);
    exprBox = new QTextEdit(this);
    resultBox = new QTextEdit(this);

    gb = new QGroupBox(this);
    mainLayout = new QVBoxLayout;
    gbL = new QHBoxLayout;
    upperRow = new QHBoxLayout;
    submitRow = new QHBoxLayout;
    resultRow = new QHBoxLayout;
    bottomRow = new QHBoxLayout;

    upperRow->addWidget(expression);
    upperRow->addWidget(exprBox);

    gbL->addWidget(interpButton);
    gbL->addWidget(printButton);
    gb->setLayout(gbL);

    submitRow->addWidget(submit);

    resultRow->addWidget(result);
    resultRow->addWidget(resultBox);

    bottomRow->addWidget(reset);

    mainLayout->addItem(upperRow);
    mainLayout->addWidget(gb);
    mainLayout->addItem(submitRow);
    mainLayout->addItem(resultRow);
    mainLayout->addItem(bottomRow);

    setLayout(mainLayout);

    connect(reset, &QPushButton::clicked, this, &Widget::resetPushed);
    connect(submit, &QPushButton::clicked, this, &Widget::submitPushed);
}

void Widget::submitPushed() {
    try {
    std::string ex = exprBox->toPlainText().toStdString();
    std::stringstream string = std::stringstream(ex);
    QString result;
    if(interpButton->isChecked()) {
        PTR(Expr) e = parseExpr(string);
        PTR(EmptyEnv) env = NEW(EmptyEnv)();
        result = QString::fromStdString(e->interp(env)->toString());
    }
    else if (printButton->isChecked()) {
        PTR(Expr) e = parseExpr(string);
        result = QString::fromStdString(e->toString());
    }
    else {
        result = "Please choose an action";
    }
    resultBox->setPlainText(result);
    }
    catch (...) {
    QString error = "error";
    resultBox->setPlainText(error);
    }
}

void Widget::resetPushed(){
    interpButton->setAutoExclusive(0);
    interpButton->setChecked(false);
    interpButton->setAutoExclusive(1);

    printButton->setAutoExclusive(0);
    printButton->setChecked(false);
    printButton->setAutoExclusive(1);

    exprBox->clear();
    resultBox->clear();
}

Widget::~Widget()
{
}

