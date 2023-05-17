import java.util.ArrayList;

public class Main {
    public static void main(String[] args) {

        for (int i = 0; i < 20; i++) {
            int rolls = Dice.rollDice();
            System.out.println(rolls);
        }
    }
}