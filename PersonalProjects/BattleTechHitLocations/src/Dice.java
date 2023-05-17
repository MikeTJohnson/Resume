import java.util.ArrayList;
import java.util.Random;

public class Dice {

    public static int rollDice() {
        Random rand = new Random();
        int diValue1 = rand.nextInt(6) + 1;
        int diValue2 = rand.nextInt(6) + 1;
        return diValue1 + diValue2;
    }
}
