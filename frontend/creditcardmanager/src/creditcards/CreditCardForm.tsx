import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { CreditCard } from "../types/creditcard";
import { omit } from "lodash"

type Args = {
    card: CreditCard;
    submitted: (card: CreditCard) => void;
}

const CreditCardForm = ({ card, submitted }: Args) => {

    const cvcExpr = new RegExp("^\\d{3}$", "g");

    const cardNumberExpr = new RegExp("^\\d{14,16}$", "g");

    const expiryDateExpr = new RegExp("^\\d{4}-\\d{2}-\\d{2}$", "g");

    const [cardState, setCardState] = useState({ ...card });

    const [errors, setErrors] = useState({});

    const navigate = useNavigate();

    const onSubmit: React.MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault();
        if (Object.keys(errors).length === 0) {
            submitted(cardState);
        }
    };

    const onCancel: React.MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault();
        navigate("/");
    };

    const validate = (element: string, value: any) => {
        switch (element) {
            case "name":
                if (value && value.length > 50) {
                    setErrors({ ...errors, name: "Card name should alphanumeric with maximum of 50 characters" })
                }
                else {
                    let updatedErr = omit(errors, "name");
                    setErrors(updatedErr);
                }
                break;
            case "cvc":
                if (!cvcExpr.test(value)) {
                    setErrors({ ...errors, cvc: "CVC should be 3-digit number" })
                }
                else {
                    let updatedErr = omit(errors, "cvc");
                    setErrors(updatedErr);
                }
                break;
            case "cardNumber":
                if (!cardNumberExpr.test(value)) {
                    setErrors({ ...errors, cardNumber: "Card number should between 14 to 16 digits" })
                }
                else {
                    let updatedErr = omit(errors, "cardNumber");
                    setErrors(updatedErr);
                }
                break;
            case "expiryDate":
                if (!expiryDateExpr.test(value)) {
                    setErrors({ ...errors, expiryDate: "ExpiryDate should be a valid date in the format YYYY-DD-MM" })
                }
                else {
                    let updatedErr = omit(errors, "expiryDate");
                    setErrors(updatedErr);
                }
                break;
            default:
                break;
        }
    };


    const onChange: React.ChangeEventHandler<HTMLInputElement> = async (e) => {
        e.persist();
        let name = e.target.name;
        let val = e.target.value;
        validate(name, val);
        setCardState({
            ...cardState,
            [name]: val
        })
    };

    return (
        <form className="mt-2">
            <div className='row mb-2'>
                <h5 className="themeFontColor text-center">
                    Register New Credit Card
                </h5>
            </div>
            <div className="form-group">
                <label htmlFor="name">Name</label>
                {
                    errors.name && <h4>{errors.name}</h4>
                }
                <input type="text"
                    className="form-control"
                    placeholder="Name"
                    value={cardState.name}
                    name="name"
                    onChange={onChange}>
                </input>
            </div>
            <div className="form-group">
                <label htmlFor="cardNumber">Card Number</label>
                {
                    errors.cardNumber && <h4>{errors.cardNumber}</h4>
                }
                <input type="text"
                    className="form-control"
                    placeholder="14-16 Digit Number"
                    value={cardState.cardNumber}
                    name="cardNumber"
                    onChange={onChange}>
                </input>
            </div>
            <div className="form-group">
                <label htmlFor="cvc">CVC</label>
                {
                    errors.cvc && <h4>{errors.cvc}</h4>
                }
                <input type="text"
                    className="form-control"
                    placeholder="3 Digit Number"
                    value={cardState.cvc}
                    name="cvc"
                    onChange={onChange}>
                </input>
            </div>
            <div className="form-group">
                <label htmlFor="expiryDate">ExpiryDate</label>
                {
                    errors.expiryDate && <h4>{errors.expiryDate}</h4>
                }
                <input type="text"
                    className="form-control"
                    placeholder="YYYY-DD-MM"
                    value={cardState.expiryDate}
                    name="expiryDate"
                    onChange={onChange}>
                </input>
            </div>
            <button className="btn btn-primary mt-2" onClick={onSubmit}>
                Submit
            </button>&nbsp;&nbsp;&nbsp;&nbsp;
            <button className="btn btn-secondary mt-2" onClick={onCancel}>
                Cancel
            </button>
        </form>
    )
};

export default CreditCardForm;