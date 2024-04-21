interface ISearchFormProps {
    placeholder: string;
    onChange: (event: React.ChangeEvent) => void;
    onSubmit: (event: React.FormEvent) => void;
    className?: string;
}

function SearchForm({ placeholder, onChange, onSubmit, className = "" }: ISearchFormProps) {
    return (
        <form className={`search-form ${className}`} onSubmit={onSubmit}>
            <input
                type="text"
                className="search-form__input"
                placeholder={placeholder}
                onChange={onChange}
            />
            <button type="submit" className="search-form__button button-violet icon-search"></button>
        </form>
    )
}

export default SearchForm;