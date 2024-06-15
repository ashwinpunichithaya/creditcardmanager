import { useNavigate } from "react-router-dom";
import config from "../config";
import { CreditCard } from "../types/creditcard";

const useRegisterCard = () => {
    const navigate = useNavigate();
    const postCard = async (card: CreditCard) => {
        const response = await fetch(`${config.baseApiUrl}/creditcards`, {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(card)
        });
        const json = await response.json();
        if (response.ok) {
            navigate("/cards");
        }
        else {
            console.log(json);
        }
    };
    return { postCard };
};

export default useRegisterCard;