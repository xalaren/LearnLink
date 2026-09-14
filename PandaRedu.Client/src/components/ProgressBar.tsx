function ProgressBar(props: { progress: number, className?: string }) {
    return (
        <div className={`progress-bar ${props.className}`}>
            <div className="progress-bar__progress" style={{ width: `${props.progress}%` }}></div>
        </div>
    );
}

export default ProgressBar;