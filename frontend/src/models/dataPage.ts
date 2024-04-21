export class DataPageHeader {
    pageSize: number;
    pageNumber: number;

    constructor(pageSize: number, pageNumber: number) {
        this.pageSize = pageSize;
        this.pageNumber = pageNumber;
    }
}

export class DataPage extends DataPageHeader {
    itemsCount: number;
    pageCount: number;

    constructor(pageSize: number, pageNumber: number, itemsCount: number, pageCount: number) {
        super(pageSize, pageNumber);
        this.itemsCount = itemsCount;
        this.pageCount = pageCount;
    }
}

export class ValueDataPage<T> extends DataPage {
    values?: T;

    constructor(pageSize: number, pageNumber: number, itemsCount: number, pageCount: number, values?: T) {
        super(pageSize, pageNumber, itemsCount, pageCount);
        this.values = values;
    }
}