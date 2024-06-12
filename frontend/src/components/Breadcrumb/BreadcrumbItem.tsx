import { useHistoryNavigation } from "../../hooks/historyNavigation";

interface IBreadcrumbItemProps {
    text: string;
    path?: string;
}

function BreadcrumbItem({ text, path }: IBreadcrumbItemProps) {
    const { toNext } = useHistoryNavigation();
    return (
        <div className="breadcrumb__item">
            <a onClick={() => {
                if (path) toNext(path);
            }}>{text}</a>
        </div>

    );
}

export default BreadcrumbItem;