import java.util.ArrayList;
import java.util.Collections;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) {

        ArrayList<String> names = new ArrayList<>();
        Scanner input = new Scanner(System.in);
        String name = input.nextLine();
        while (!name.equals("done")) {
            names.add(name);
            name = input.nextLine();
        }
        for (int i = 0; i < 5; i++) {
            Collections.shuffle(names);
        }
        for (int i = 0; i < names.size(); i++) {
            System.out.println(i + " " + names.get(i));
        }
    }
}