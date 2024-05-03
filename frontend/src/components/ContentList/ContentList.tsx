interface IContentListProps {
    title: string;
    showButton: boolean;
    children: React.ReactNode;
    onHeadButtonClick?: () => void;
    className?: string;
}

function ContentList({ title, showButton, children, onHeadButtonClick, className = '' }: IContentListProps) {
    return (
        <div className={`content-list ${className}`}>
            <div className="content-list__head">
                <h3>{title}</h3>
                {showButton &&
                    <button
                        className="content-list__add-button button-gray icon-plus icon-medium-size"
                        onClick={onHeadButtonClick}>
                    </button>
                }
            </div>

            <div className="content-list__items">
                {children}
            </div>
        </div>

    );
}

export default ContentList;