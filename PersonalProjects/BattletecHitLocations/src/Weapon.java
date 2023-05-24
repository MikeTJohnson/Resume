public class Weapon {

    int lRange;
    int mRange;
    int sRange;
    int minRange;
    int heat;

    public boolean hit(int skill, int range, int extraPen){
        int roll = Dice.rollDice();
        int shortRangePen = skill + extraPen;
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
}
