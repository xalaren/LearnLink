interface ISearchFormProps {
    placeholder: string;
    value: string;
    onChange: (event: React.ChangeEvent) => void;
    onSubmit: (event: React.FormEvent) => void;
    className?: string;
}

function SearchForm({ placeholder, value, onChange, onSubmit, className = "" }: ISearchFormProps) {
    return (
        <form className={`search-form ${className}`} onSubmit={onSubmit}>
            <input
                type="text"
                className="search-form__input"
                placeholder={placeholder}
                onChange={onChange}
                value={value}
            />
            <button type="submit" className="search-form__button button-violet icon-search"></button>
        </form>
    )
}

export default SearchForm;