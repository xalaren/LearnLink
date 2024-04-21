interface ISearchFormProps {
    onChange: (event: React.ChangeEvent) => void;
    onSubmit: (event: React.FormEvent) => void;
    className?: string;
}

function SearchForm({ onChange, onSubmit, className = "" }: ISearchFormProps) {
    return (
        <form className={`search-form ${className}`} onSubmit={onSubmit}>
            <input
                type="text"
                className="search-form__input"
                placeholder="Найти по названию курса или категории..."
                onChange={onChange}
            />
            <button type="submit" className="search-form__button button-violet icon-search"></button>
        </form>
    )
}

export default SearchForm;