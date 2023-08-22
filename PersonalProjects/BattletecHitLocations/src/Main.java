import java.util.ArrayList;

public class Main {
    public static void main(String[] args) {

        Weapon ac2 = new AC2();
        Weapon medium = new MediumLaser();
        for (int i = 0; i < 20; i++) {
            System.out.println(ac2.fire("right", 4, 6, 0));
            System.out.println(medium.fire("right", 4, 6, 0));
        }
    }
}