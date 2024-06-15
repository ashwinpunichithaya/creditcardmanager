import useRegisterCard from "../hooks/useRegisterCard";
import { CreditCard } from "../types/creditcard";
import CreditCardForm from "./CreditCardForm";

const RegisterCreditCard = () => {
    const registerCard = useRegisterCard();
    const card: CreditCard = {
        id: undefined,
        name: "",
        cardNumber: "",
        cvc: "",
        expiryDate: ""
    }

    return (
        <CreditCardForm card={card} submitted={(card: CreditCard) => {registerCard.postCard(card)}}/>
    )
}

export default RegisterCreditCard;