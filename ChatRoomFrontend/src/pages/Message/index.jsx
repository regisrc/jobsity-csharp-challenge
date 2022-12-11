import { Container, ChatRoomCard, MessageContainer, UserName, MessageWrited, Input, Button } from "./styles";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import useSWR from 'swr';

const Message = () => {
  const navigate = useNavigate();

  const api = axios.create({
    baseURL: 'https://localhost:7233',
  });

  const useSwr = (url) => {
    const { data, error, mutate } = useSWR(url, async (url) => {
      const response = await api.get(url);
  
      return response.data;
    });
  
    return { data, error, mutate };
  };

  const { data } = useSwr(`Message?chatRoomId=${sessionStorage.getItem("chatRoomId")}&token=${sessionStorage.getItem("token")}`);
  const [userId, setUserId] = useState(sessionStorage.getItem("id"));
  const [message, setMessage] = useState();

  const SendMessage = () => {
    const body = {
      message: message,
      chatRoomId: sessionStorage.getItem("chatRoomId"),
      userId: userId
    }

    axios
      .post(
        `https://localhost:7233/Message?token=${sessionStorage.getItem("token")}`, body
      )
      .then((response) => {
        setMessage("")
      });
  }

  const SetName = (id, name) => {
    return id === userId ? `:${name}` : `${name}:`
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
      })
      .catch((response) => {
        navigate("/");
      });
  }, []);

  return (
    <Container>
      <ChatRoomCard>
        
      {data?.map((x) => (
        <MessageContainer direction={userId === x.userId ? "row-reverse" : "row"}>
          <UserName userColor={userId === x.userId ? '#5634f8' : '#fcb122'}>{SetName(x.userId, x.name)} </UserName>
          <MessageWrited>{x.message}</MessageWrited>
        </MessageContainer>
      ))}
      </ChatRoomCard>
      <Input type="text" value={message} onChange={e => setMessage(e.target.value)}/>
      <Button type="button" value="Send" onClick={SendMessage} />
    </Container>
  );
};

export default Message;
