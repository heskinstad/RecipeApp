import './style.css';

function CommentBlock({comment}) {

    const date = new Date(comment.createdAt);

    const time = date.toLocaleTimeString([], {
    hour: "2-digit",
    minute: "2-digit",
    hour12: false
    });

    const datePart = date.toLocaleDateString("en-US", {
    year: "numeric",
    month: "long",
    day: "numeric"
    });

    const datetime = `${time} ${datePart}`;

    return (
        <div className="commentBlock">
            <div className="user">
                {comment.userName} says:
            </div>
            <div className="date">
                {datetime}
            </div>
            <div className="comment">
                {comment.message}
            </div>
            
            <br />
            
        </div>
    );
}

export default CommentBlock;