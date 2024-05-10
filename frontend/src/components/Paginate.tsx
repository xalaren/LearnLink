interface IPaginateProps {
    currentPage: number;
    pageCount: number;
    setPage: (page: number) => void;
}

function Paginate({ currentPage, pageCount, setPage }: IPaginateProps) {
    const pages: number[] = [];

    for (let i = 1; i < pageCount + 1; i++) {
        pages.push(i);
    }

    return (
        <>

            {pageCount > 0 &&
                <div className="pagination-form">
                    <button className="pagination-form__button button-violet-light icon-arrow-left" onClick={() => {
                        setPage(calculatePage(currentPage, pageCount, false));
                    }}></button>

                    <PaginateMiddleButtons currentPage={currentPage} pages={pages} setPage={setPage} />

                    <button className="pagination-form__button button-violet-light icon-arrow-right" onClick={() => {
                        setPage(calculatePage(currentPage, pageCount, true));
                    }}></button>
                </div>

            }
        </>

    );
}

function PaginateMiddleButtons(props: { currentPage: number, pages: number[], setPage: (page: number) => void }) {
    return (
        <>
            {
                props.pages.map(page => {
                    const selectedClassName = page === props.currentPage ? 'button-violet-light-selected' : '';
                    return (
                        <button
                            className={`pagination-form__button button-violet-light ${selectedClassName}`}
                            key={page}
                            onClick={() => {
                                props.setPage(page)
                            }}>
                            {page}
                        </button>
                    )
                })

            }
        </>
    );
}

function calculatePage(page: number, count: number, next: boolean): number {
    if (next && page + 1 > count) {
        return page;
    }

    if (!next && page - 1 < 1) return page;

    const modifier = next ? 1 : -1;

    return page + modifier;
}

export default Paginate;