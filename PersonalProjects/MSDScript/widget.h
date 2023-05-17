#ifndef WIDGET_H
#define WIDGET_H

#include <QWidget>
#include <QtWidgets>

class Widget : public QWidget
{
    Q_OBJECT

    QLabel *expression;
    QLabel *result;
    QRadioButton *interpButton;
    QRadioButton *printButton;
    QPushButton *submit;
    QPushButton *reset;
    QTextEdit *exprBox;
    QTextEdit *resultBox;
    QVBoxLayout *mainLayout;
    QHBoxLayout *exprLayout;
    QHBoxLayout *resultLayout;
    QGroupBox *gb;
    QHBoxLayout *gbL;
    QHBoxLayout *upperRow;
    QHBoxLayout *submitRow;
    QHBoxLayout *resultRow;
    QHBoxLayout *bottomRow;

public:
    Widget(QWidget *parent = nullptr);
    ~Widget();

public slots:
    void submitPushed();
    void resetPushed();
};
#endif // WIDGET_H
