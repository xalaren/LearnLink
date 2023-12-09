export function NoMatch() {
    const styles = {
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        height: "100%"
    }
    return(
        <main className="main container">
            <div style = {{ height: "100%"}} className="inner-container">
                <h2 style={styles}>Такого пути не существует :(</h2>
            </div>
        </main>
    );
}