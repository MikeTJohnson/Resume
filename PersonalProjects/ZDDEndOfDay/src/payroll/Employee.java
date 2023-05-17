package payroll;

import java.util.ArrayList;

public class Employee {

    String name_;
    String empNum_;
    float rate_;
    float[] hours_ = new float[10];
    //todo hours array and calculate pay method

    static ArrayList<Employee> employeeList = new ArrayList<>();


    public Employee(String name, String empNum, float rate) {
        name_ = name;
        empNum_ = empNum;
        rate_ = rate;
    }


    private void growArray() {
        float[] temp = new float[hours_.length * 2];
        for (int i = 0; i < hours_.length; i++) {
            temp[i] = hours_[i];
        }
        hours_ = temp;
    }
}
