import { Container, ChatRoomCard, Name, Icon, Feedback } from "./styles";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const ChatRoom = () => {
  const navigate = useNavigate();
  const [chatRooms, setChatRooms] = useState([]);

  const GetChatRooms = () => {
    axios
      .get(
        `https://localhost:7233/Chatroom?token=${sessionStorage.getItem(
          "token"
        )}`
      )
      .then((response) => {
        setChatRooms(response.data);
      });
  };

  const GoToChatRoom = (id) => {
    sessionStorage.setItem("chatRoomId", id)

    navigate("/chatRoom");
  }

  useEffect(() => {
    axios
      .get(
        `https://localhost:7233/User/verifyToken?token=${sessionStorage.getItem(
          "token"
        )}`
      )
      .then((response) => {
        if (!response.data.success) {
          navigate("/");
        }

        if (response.data.success) {
          GetChatRooms();
        }
      })
      .catch((response) => {
        navigate("/");
      });
  }, []);

  return (
    <Container>
      {chatRooms.length === 0 ? <Feedback>Go to swagger and add chatroom</Feedback> : <></>}
      {chatRooms.map((x) => (
        <ChatRoomCard key={x.creatorId} onClick={() => GoToChatRoom(x.id)}>
          <Name>{x.name}</Name>
          <Icon />
        </ChatRoomCard>
      ))}
    </Container>
  );
};

export default ChatRoom;
