interface ISearchFormProps {
    className?: string;
}

function SearchForm({ className = "" }: ISearchFormProps) {
    return (
        <form action="#" className={`search-form ${className}`}>
            <input type="text" className="search-form__input" placeholder="Найти по названию курса или категории..." />
            <button className="search-form__button button-violet icon-search"></button>
        </form>
    )
}

export default SearchForm;