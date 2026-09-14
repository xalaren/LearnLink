interface RateSelectorProps {
    minRate: number;
    maxRate: number;
    selectedRateValue: number;
    setSelectedRateValue: (value: number) => void;
}

function RateSelector({ minRate, maxRate, selectedRateValue, setSelectedRateValue }: RateSelectorProps) {
    const values: number[] = [];

    for (let i = minRate; i < maxRate + 1; i++) {
        values.push(i);
    }

    return (
        <div className="rate-selector">
            {
                values.map(value => {
                    const selectedClassName = value === selectedRateValue ? 'button-violet-light-selected' : '';
                    return (
                        <button
                            className={`rate-selector__button  button-violet-light ${selectedClassName}`}
                            key={value}
                            type="button"
                            onClick={() => {
                                setSelectedRateValue(value)
                            }}>
                            {value}
                        </button>
                    )
                })
            }
        </div>
    );
}

export default RateSelector;