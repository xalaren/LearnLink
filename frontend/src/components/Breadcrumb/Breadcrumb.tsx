function Breadcrumb({ children }: { children: React.ReactNode }) {
    return (
        <ol className="breadcrumb">
            {children}
        </ol>
    );
}

export default Breadcrumb;