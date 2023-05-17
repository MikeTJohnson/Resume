import java.util.ArrayList;

public class BillingUtil {

    public static int getPartQuantity(ArrayList<FinishedParts> finishedPart, String partNumber) {
        int total = 0;
        for (FinishedParts part: finishedPart) {
            if (partNumber.equals(part.getPartNumber())) {
                total += part.getQuantity();
            }
        }
        return total;
    }
}
