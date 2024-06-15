import useFetchRegisteredCards from "../hooks/useFetchRegisteredCards";

const CreditCardList = () => {
    const creditcards = useFetchRegisteredCards();
    return (
        <div>
            <div className='row mb-2'>
                <h5 className="themeFontColor text-center">
                    Registered Credit Cards
                </h5>
            </div>
            <table className="table table-hover">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>ExpiryDate</th>
                    </tr>
                </thead>
                <tbody>
                    {creditcards.map((c) =>
                    (
                        <tr key={c.id}>
                            <td>{c.name}</td>
                            <td>{c.expiryDate}</td>
                        </tr>
                    )
                    )}
                </tbody>
            </table>
        </div>
    )
};

export default CreditCardList;