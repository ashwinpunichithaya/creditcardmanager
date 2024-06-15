import { useEffect, useState } from "react";
import config from "../config";
import { CreditCard } from "../types/creditcard";

const useFetchRegisteredCards = (): CreditCard[] => {
    const [creditcards, setCreditCards] = useState<CreditCard[]>([]);
    useEffect(() => {
        const fetchRegisteredCards = async () => {
            const response = await fetch(`${config.baseApiUrl}/creditcards`);
            const creditcards = await response.json();
            setCreditCards(creditcards);
        };
        fetchRegisteredCards();
    }, [])
    return creditcards;
};

export default useFetchRegisteredCards;