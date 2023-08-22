public class ClusterWeapon extends Weapon {
    //todo get a method for calculating cluster hits
    int damagePerShot;
    @Override
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
