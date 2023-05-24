import java.util.Random;
import java.util.Scanner;

public class Main {

    public static int rollDie(Random rand) {;
        return (rand.nextInt(6) % 6) + 1;
    }

    public static void wound(int attacks, int skill, int woundOn) {
        Random rand = new Random();
        int hits = 0;
        int hitSix = 0;
        int wounds = 0;
        int woundSix = 0;
        int roll = 0;
        for (int i = 0; i < attacks; i++) {
            roll = rollDie(rand);
            if (roll >= skill) {
                hits++;
                if (roll == 6) {
                    hitSix++;
                }
            }
        }
        for (int i = 0; i < hits; i++) {
            roll = rollDie(rand);
            if (roll >= woundOn) {
                wounds++;
                if (roll == 6) {
                    woundSix++;
                }
            }
        }
        System.out.println("sixes on hit " + hitSix);
        System.out.println("hits: " + hits);
        System.out.println("sixes on wound " + woundSix);
        System.out.println("wounds: " + wounds);
    }

    public static void main(String[] args) {
        Scanner input = new Scanner(System.in);
        System.out.println("How many attack");
        int attacks = input.nextInt();
        System.out.println("What is the skill");
        int skill = input.nextInt();
        System.out.println("What do you need to roll to wound");
        int woundOn = input.nextInt();
        wound(attacks, skill, woundOn);
    }
}