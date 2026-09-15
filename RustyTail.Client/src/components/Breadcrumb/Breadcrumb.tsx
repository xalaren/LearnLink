function Breadcrumb({ children }: { children: React.ReactNode }) {
    return (
        <nav className="breadcrumb">
            {children}
        </nav>
    );
}

export default Breadcrumb;