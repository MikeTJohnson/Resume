import java.util.ArrayList;

public class Weapon {

    String name;
    int lRange;
    int mRange;
    int sRange;
    int minRange;
    int heat;
    int damage;


    public boolean hit(int skill, int range, int extraPenalty){
        int roll = Dice.rollDice();
        int shortRangePen = skill + extraPenalty;
        if (range < minRange) {
            return roll > shortRangePen + ((minRange + 1) - range);
        }
        else if (range <= sRange){
            return roll > shortRangePen;
        }
        else if (range <= mRange) {
            return roll > (shortRangePen + 2);
        }
        else if (range <= lRange) {
            return roll > (shortRangePen + 4);
        }
        else {
            return false;
        }
    }

    public String hitLocation(String arc, int roll) {
        String location = "";
        if (arc.equals("front")) {
            if (roll == 2) { location = "Center torso through armor critical";}
            if (roll == 3) { location =  "Right arm";}
            if (roll == 4) { location =  "Right arm";}
            if (roll == 5) { location =  "Right leg";}
            if (roll == 6) { location =  "Right torso";}
            if (roll == 7) { location =  "Center torso";}
            if (roll == 8) { location =  "Left torso";}
            if (roll == 9) { location =  "Left leg";}
            if (roll == 10) { location =  "Left arm";}
            if (roll == 11) { location =  "Left arm";}
            if (roll == 12) { location =  "Head";}
        }
        else if (arc.equals("right")) {
            if (roll == 2) { location = "Right torso through armor critical";}
            if (roll == 3) { location =  "Right leg";}
            if (roll == 4) { location =  "Right arm";}
            if (roll == 5) { location =  "Right arm";}
            if (roll == 6) { location =  "Right leg";}
            if (roll == 7) { location =  "Right torso";}
            if (roll == 8) { location =  "Center torso";}
            if (roll == 9) { location =  "Left torso";}
            if (roll == 10) { location =  "Left arm";}
            if (roll == 11) { location =  "Left leg";}
            if (roll == 12) { location =  "Head";}
        }
        else if (arc.equals("left")) {
            if (roll == 2) { location = "Left torso through armor critical";}
            if (roll == 3) { location =  "Left leg";}
            if (roll == 4) { location =  "Left arm";}
            if (roll == 5) { location =  "Left arm";}
            if (roll == 6) { location =  "Left leg";}
            if (roll == 7) { location =  "Left torso";}
            if (roll == 8) { location =  "Center torso";}
            if (roll == 9) { location =  "Right torso";}
            if (roll == 10) { location =  "Right arm";}
            if (roll == 11) { location =  "Right leg";}
            if (roll == 12) { location =  "Head";}
        }
        else {
            if (roll == 2) { location = "Center torso through armor critical rear";}
            if (roll == 3) { location =  "Right arm";}
            if (roll == 4) { location =  "Right arm";}
            if (roll == 5) { location =  "Right leg";}
            if (roll == 6) { location =  "Right torso rear";}
            if (roll == 7) { location =  "Center torso rear";}
            if (roll == 8) { location =  "Left torso rear";}
            if (roll == 9) { location =  "Left leg";}
            if (roll == 10) { location =  "Left arm";}
            if (roll == 11) { location =  "Left arm";}
            if (roll == 12) { location =  "Head";}
        }
        return location;
    }

    public String fire(String arc, int skill, int range, int extraPenalty) {
        String result = name;
        if (hit(skill, range, extraPenalty)) {
            result += " hit " + hitLocation(arc, Dice.rollDice()) + " for " + damage + " damage.";
        }
        else {
            result += " missed.";
        }
        return result;
    }
}
